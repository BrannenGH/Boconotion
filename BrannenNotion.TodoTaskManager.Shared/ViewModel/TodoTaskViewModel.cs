namespace BrannenNotion.TodoTaskManager.ViewModel
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using BrannenNotion.Shared;
    using BrannenNotion.TodoTaskManager.Shared.Model;

    public class TodoTaskViewModel
     {
         public ObservableCollection<TodoTask> TodoTasks = new ObservableCollection<TodoTask>();
         private TaskRepository _taskRepository;

         public TodoTaskViewModel(TaskRepository taskRepository)
         {
             this._taskRepository = taskRepository;
         }

         public async Task LoadTasks()
         {
              var todoTaskPages = (await this._taskRepository.GetTasks()).Results;
              var todoTasks = todoTaskPages.Select(tt => TodoTask.CreateFromPage(tt));
              foreach (var todoTask in todoTasks)
              {
                  this.TodoTasks.Add(todoTask);
              }
         }

         public async Task UpdateTasks()
         {
             var tasksToUpdate = TodoTasks.Where(task => task.NeedsUpdate);
             foreach (var todoTask in tasksToUpdate)
             {
                 await this._taskRepository.UpdateTodoTask(todoTask.TodoTaskId, todoTask.Title, todoTask.Checked);
                 todoTask.NeedsUpdate = false;
             }
         }
     }
}



