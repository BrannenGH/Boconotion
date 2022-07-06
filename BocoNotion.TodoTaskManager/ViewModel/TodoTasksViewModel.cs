namespace BocoNotion.TodoTaskManager.ViewModel
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using BocoNotion.Shared;
    using BocoNotion.Shared.Model;
    using Microsoft.Toolkit.Mvvm.ComponentModel;
    using Microsoft.Toolkit.Mvvm.Input;
    using Notion.Client;

    /// <summary>
    /// View model for the <see cref="TodoTaskPage"/>.
    /// </summary>
    public class TodoTasksViewModel : ObservableObject
    {
        private TaskRepository taskRepository;

        public ObservableCollection<TodoTaskViewModel> TodoTasks = new ObservableCollection<TodoTaskViewModel>();

        public ICommand LoadTasksCommand { get; }
        public ICommand UpdateTasksCommand { get; }
        public ICommand AddTaskCommand { get; }

        public string NewTaskTitle
        {
            get => this.newTaskTitle;
            set
            {
                this.SetProperty(ref this.newTaskTitle, value);
                EvaluateCanAddTask();
            }
        }

        private string newTaskTitle = "";

        public bool CanAddTask
        {
            get => this.canAddTask;
            set => this.SetProperty(ref this.canAddTask, value);
        }

        private bool canAddTask = false;

        public TodoTasksViewModel(string token)
        {
            this.ConfigureClient(token);
            this.LoadTasksCommand = new AsyncRelayCommand(this.LoadTasks);
            this.UpdateTasksCommand = new AsyncRelayCommand(this.UpdateTasks);
            this.AddTaskCommand = new AsyncRelayCommand(this.AddTask);
        }

        public void ConfigureClient(string token)
        {
            var client = NotionClientFactory.Create(new ClientOptions
            {
                AuthToken = token,
            });

            this.taskRepository = new TaskRepository(client);
        }

        public async Task LoadTasks()
        {
            var todoTasks = (await this.taskRepository.GetTasks()).Tasks;
            var viewModels = todoTasks.Select(x => new TodoTaskViewModel(x));
            this.TodoTasks.Clear();
            foreach (var todoTask in viewModels.OrderBy(x => x.Checked))
            {
                this.TodoTasks.Add(todoTask);
            }
        }

        public async Task UpdateTasks()
        {
            var tasksToUpdate = TodoTasks.Where(task => task.NeedsUpdate);
            foreach (var todoTask in tasksToUpdate)
            {
                await this.taskRepository.UpdateTodoTask(todoTask.TodoTask);
                todoTask.OnUpdate();
            }
            await this.LoadTasks();
        }

        public async Task AddTask()
        {
            this.CanAddTask = false;
            await this.taskRepository.AddTodoTask(new TodoTask { Title = this.NewTaskTitle });
            this.NewTaskTitle = "";
            await this.LoadTasks();
            this.CanAddTask = true;
        }

        public void EvaluateCanAddTask()
        {
            this.CanAddTask = !string.IsNullOrWhiteSpace(this.NewTaskTitle);
        }
    }
}
