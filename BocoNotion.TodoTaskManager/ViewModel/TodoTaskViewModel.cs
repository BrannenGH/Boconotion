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

    public class TodoTaskViewModel : ObservableObject
    {
        /// <summary>
        /// Gets or sets a value indicating whether the todo item needs to be updated in Notion.
        /// </summary>
        public bool NeedsUpdate { get; private set; } = false;

        public string Id => this.TodoTask.Id;

        public TodoTask TodoTask { get; }

        /// <summary>
        /// Gets or sets the name of the todo item.
        /// </summary>
        public string Title
        {
            get
            {
                return this.TodoTask.Title;
            }

            set
            {
                this.TodoTask.Title = value;
                this.NeedsUpdate = true;
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether the todo item is finished.
        /// </summary>
        public bool Checked
        {
            get
            {
                return this.TodoTask.Checked;
            }

            set
            {
                this.TodoTask.Checked = value;
                this.NeedsUpdate = true;
            }
        }

        public TodoTaskViewModel(TodoTask tt)
        {
            this.TodoTask = tt;
        }

        public void OnUpdate()
        {
            this.NeedsUpdate = false;
        }

        public static TodoTaskViewModel CreateFromPage(Page todoTaskPage)
        {

            var tt = new TodoTask
            {
                Id = todoTaskPage.Id,
                Title = (todoTaskPage.Properties["Name"] as TitlePropertyValue)?.Title?.First()?.PlainText,
                Checked = (todoTaskPage.Properties["State"] as SelectPropertyValue)?.Select?.Name == "Done",
            };

            return new TodoTaskViewModel(tt);
        }
    }
}

