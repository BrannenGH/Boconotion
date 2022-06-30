namespace BrannenNotion.TodoTaskManager.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using BrannenNotion.Shared;
    using BrannenNotion.TodoTaskManager.Shared.Model;
    using Microsoft.Toolkit.Mvvm.ComponentModel;
    using Microsoft.Toolkit.Mvvm.Input;
    using Notion.Client;

    /// <summary>
    /// View model for the <see cref="TodoTaskPage"/>.
    /// </summary>
    [Bindable(true)]
    public class TodoTaskViewModel : ObservableObject
    {
        private string token;
        private TaskRepository taskRepository;

        [Bindable(true)]
        public ObservableCollection<TodoTask> TodoTasks = new ObservableCollection<TodoTask>();

        [Bindable(true)]
        public ICommand LoadTasksCommand { get; }

        [Bindable(true)]
        public ICommand SaveTokenCommand { get; }

        [Bindable(true)]
        public string Token
        {
            get => this.token;
            set
            {
                this.SetProperty(ref this.token, value);
                this.ConfigureClient(value);
            }
        }

        public TodoTaskViewModel()
        {
            this.LoadTasksCommand = new AsyncRelayCommand(this.LoadTasks);
            this.SaveTokenCommand = new AsyncRelayCommand(this.SubmitToken);
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
            try
            {
                var todoTaskPages = (await this.taskRepository.GetTasks()).Results;
                var todoTasks = todoTaskPages.Select(tt => TodoTask.CreateFromPage(tt));
                foreach (var todoTask in todoTasks)
                {
                    this.TodoTasks.Add(todoTask);
                }
            }
            catch (Exception e)
            {
            }
        }

        public async Task UpdateTasks()
        {
            var tasksToUpdate = TodoTasks.Where(task => task.NeedsUpdate);
            foreach (var todoTask in tasksToUpdate)
            {
                await this.taskRepository.UpdateTodoTask(todoTask.TodoTaskId, todoTask.Title, todoTask.Checked);
                todoTask.NeedsUpdate = false;
            }
        }

        public async Task SubmitToken()
        {
            this.ConfigureClient(this.Token);
            await this.LoadTasks();
        }
    }
}
