namespace BocoNotion.TodoTaskManager.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using BocoNotion.Shared;
    using BocoNotion.TodoTaskManager.Shared.Model;
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
        public ICommand UpdateTasksCommand { get;  }

        public TodoTasksViewModel(string token)
        {
            this.ConfigureClient(token);
            this.LoadTasksCommand = new AsyncRelayCommand(this.LoadTasks);
            this.UpdateTasksCommand = new AsyncRelayCommand(this.UpdateTasks);
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
            var todoTaskPages = (await this.taskRepository.GetTasks()).Results;
            var todoTasks = todoTaskPages.Select(tt => TodoTaskViewModel.CreateFromPage(tt));
            foreach (var todoTask in todoTasks.OrderBy(x => x.Checked))
            {
                this.TodoTasks.Add(todoTask);
            }
        }

        public async Task UpdateTasks()
        {
            var tasksToUpdate = TodoTasks.Where(task => task.NeedsUpdate);
            foreach (var todoTask in tasksToUpdate)
            {
                await this.taskRepository.UpdateTodoTask(todoTask.Id, todoTask.Title, todoTask.Checked);
                todoTask.OnUpdate();
            }
        }
    }
}
