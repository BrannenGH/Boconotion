namespace BocoNotion.TodoTaskManager.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using BocoNotion.Model;
    using Microsoft.Toolkit.Mvvm.ComponentModel;

    public class TodoTaskViewModel : ObservableObject
    {
        /// <summary>
        /// Gets or sets a value indicating whether the todo item needs to be updated in Notion.
        /// </summary>
        public bool NeedsUpdate
        {
            get => this.needsUpdate;
            private set => this.SetProperty(ref needsUpdate, value);
        }

        private bool needsUpdate = false;

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
                this.SetProperty(this.TodoTask.Title, value, this.TodoTask, (tt, title) => tt.Title = title);
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
                this.SetProperty(this.TodoTask.Checked, value, this.TodoTask, (tt, isChecked) => tt.Checked = isChecked);
                this.NeedsUpdate = true;
            }
        }

        public DateTime? DueDate
        {
            get => this.TodoTask.DueDate;
            set
            {
                this.SetProperty(this.TodoTask.DueDate, value, this.TodoTask, (tt, dueDate) => tt.DueDate = dueDate);
                this.NeedsUpdate = true;
            }
        }

        // TODO: Implement tracking
        public ObservableCollection<string> tags;

        public ObservableCollection<string> Tags
        {
            get => this.tags;
            set => this.SetProperty(ref this.tags, value);
        }

        public TodoTaskViewModel(TodoTask tt)
        {
            this.TodoTask = tt;

            this.Tags = new ObservableCollection<string>(tt.Tags);
        }
    
        public void OnUpdate()
        {
            this.NeedsUpdate = false;
        }
    }
}

