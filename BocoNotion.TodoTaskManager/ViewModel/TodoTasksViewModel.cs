namespace BocoNotion.TodoTaskManager.ViewModel
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using BocoNotion.Model;
    using BocoNotion.NotionIntegration;
    using BocoNotion.TodoTaskManager.Persistence;
    using Microsoft.Toolkit.Mvvm.ComponentModel;
    using Microsoft.Toolkit.Mvvm.Input;
    using Notion.Client;
    using Serilog;

    /// <summary>
    /// View model for the <see cref="TodoTaskPage"/>.
    /// </summary>
    public class TodoTasksViewModel : ObservableObject
    {
        private TaskRepository taskRepository;

        public ObservableCollection<TodoTaskViewModel> TodoTasks
        {
            get => this.todoTasks;
            set => this.SetProperty(ref this.todoTasks, value);
        }

        public ObservableCollection<TodoTaskViewModel> todoTasks = new ObservableCollection<TodoTaskViewModel>();

        public ICommand LoadTasksCommand { get; }
        public ICommand UpdateTasksCommand { get; }
        public ICommand AddTaskCommand { get; }

        public ITokenProvider tokenProvider { get; set; }
        public ILogger logger { get; set; }

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

        public TodoTasksViewModel()
        {
            this.LoadTasksCommand = new AsyncRelayCommand(this.LoadTasks);
            this.UpdateTasksCommand = new AsyncRelayCommand(this.UpdateTasks);
            this.AddTaskCommand = new AsyncRelayCommand(this.AddTask);
        }

        public async Task ConfigureClient()
        {
            var client = NotionClientFactory.Create(new ClientOptions
            {
                AuthToken = await this.tokenProvider.GetToken(),
            });

            this.taskRepository = new TaskRepository(client);
            this.taskRepository.Logger = logger;
        }

        public async Task LoadTasks()
        {
            if (this.taskRepository == null)
            {
                await this.ConfigureClient();
            }

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
            if (this.taskRepository == null)
            {
                await this.ConfigureClient();
            }

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
            if (this.taskRepository == null)
            {
                await this.ConfigureClient();
            }

            this.CanAddTask = false;
            var ttToAdd = new TodoTask { Title = this.NewTaskTitle };

            logger.Information("Adding TodoTask {@TtToAdd}", ttToAdd);
            await this.taskRepository.AddTodoTask(ttToAdd);

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
