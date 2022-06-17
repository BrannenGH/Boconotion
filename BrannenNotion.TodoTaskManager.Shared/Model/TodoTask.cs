namespace BrannenNotion.TodoTaskManager.Shared.Model
{
    using System.Linq;
    using Notion.Client;
    using Page = Notion.Client.Page;

    /// <summary>
    /// Represents a task in a todo list.
    /// </summary>
    public class TodoTask
    {
         private bool _checked = false;
         private string _title;

        /// <summary>
        /// Gets or sets the name of the todo item.
        /// </summary>
         public string Title
         {
             get
             {
                 return this._title;
             }

             set
             {
                 this._title = value;
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
                 return this._checked;
             }

             set
             {
                 this._checked = value;
                 this.NeedsUpdate = true;
             }
         }

         /// <summary>
         /// Gets or sets a value indicating whether the todo item needs to be updated in Notion.
         /// </summary>
         public bool NeedsUpdate { get; set; } = false;

         public string TodoTaskId { get; private set; }

         public static TodoTask CreateFromPage(Page todoTaskPage)
         {
            var title = (todoTaskPage.Properties["Name"] as TitlePropertyValue)?.Title.First().PlainText;
            var isChecked = (todoTaskPage.Properties["State"] as SelectPropertyValue)?.Select.Name == "Done";

            return new TodoTask
            {
                Title = title,
                Checked = isChecked,
                NeedsUpdate = false,
                TodoTaskId = todoTaskPage.Id,
            };
         }
     }
}

