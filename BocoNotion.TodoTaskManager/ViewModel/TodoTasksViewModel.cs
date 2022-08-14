namespace BocoNotion.TodoTaskManager.ViewModel
{
    using System;
    using System.Collections.Generic;
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

        public ObservableCollection<string> TodoTaskTags
        {
            get => this.todoTaskTags;
            set => this.SetProperty(ref this.todoTaskTags, value);
        }

        private string filterTagName;

        public string FilterTagName
        {
            get => this.filterTagName;
            set
            {
                this.SetProperty(ref this.filterTagName, value);
                this.FilterTasks();
            }
        }

        public ICollection<TodoTaskViewModel> allTodoTasks = new List<TodoTaskViewModel> { };

        public ObservableCollection<TodoTaskViewModel> todoTasks = new ObservableCollection<TodoTaskViewModel>();

        public ObservableCollection<string> todoTaskTags = new ObservableCollection<string>();

        public ICommand LoadTasksCommand { get; }
        public ICommand UpdateTasksCommand { get; }
        public ICommand AddTaskCommand { get; }
        public ICommand FilterTasksCommand { get; }

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

            try
            {
                var todoTasks = (await this.taskRepository.GetTasks()).Tasks;
                allTodoTasks = todoTasks.Select(x => new TodoTaskViewModel(x)).ToList();
                this.TodoTasks.Clear();
                this.todoTaskTags.Clear();
                foreach (var todoTask in allTodoTasks.OrderBy(x => x.Checked))
                {
                    this.TodoTasks.Add(todoTask);
                    foreach (var tag in todoTask.Tags)
                    {
                        if (!this.todoTaskTags.Contains(tag))
                        {
                            this.todoTaskTags.Add(tag);
                        }

                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal(e, "Could not load todotasks and tags");
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

        public void FilterTasks()
        {
            this.TodoTasks.Clear();
            foreach (var todoTask in allTodoTasks
                .Where(x => x.Tags.Contains(this.FilterTagName))
                .OrderBy(x => x.Checked)
                )
            {
                this.TodoTasks.Add(todoTask);
            }
        }

        public void EvaluateCanAddTask()
        {
            this.CanAddTask = !string.IsNullOrWhiteSpace(this.NewTaskTitle);
        }
    }
}
