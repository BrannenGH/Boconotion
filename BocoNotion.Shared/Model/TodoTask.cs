namespace BocoNotion.Shared.Model
{
    /// <summary>
    /// Represents a task in a todo list.
    /// </summary>
    public class TodoTask
    {
        /// <summary>
        /// Gets or sets the Notion provided ID for the task.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the title for the task.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the task is done or not.
        /// </summary>
        public bool Checked { get; set; }
     }
}