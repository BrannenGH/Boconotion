namespace BocoNotion.TodoTaskManager.Shared.Model
{
    using System.Linq;
    using Notion.Client;
    using Page = Notion.Client.Page;

    /// <summary>
    /// Represents a task in a todo list.
    /// </summary>
    public class TodoTask
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public bool Checked { get; set; }
     }
}

