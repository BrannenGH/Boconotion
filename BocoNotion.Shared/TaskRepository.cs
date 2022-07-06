namespace BocoNotion.Shared
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BocoNotion.Shared.Model;
    using BrannenNotion.Shared.Page;
    using Notion.Client;

    /// <summary>
    /// Repository to proxy calls to manage the Notion task database.
    /// </summary>
    public class TaskRepository
    {
        private readonly NotionClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskRepository"/> class.
        /// </summary>
        /// <param name="client">The authenticated Notion Client.</param>
        /// <param name="taskDatabaseId">Optional database ID for tasks.</param>
        public TaskRepository(NotionClient client, string taskDatabaseId = null)
        {
            this.client = client;
            this.TaskDatabaseId = taskDatabaseId;
        }

        /// <summary>
        /// Gets or sets the Notion ID for the task database.
        /// </summary>
        public string TaskDatabaseId { get; set; }

        /// <summary>
        /// Fetch the Task Notion Database ID.
        /// </summary>
        /// <returns>
        /// The Notion ID for the Task Database.
        /// </returns>
        public async Task<string> GetTaskDatabaseId()
        {
            var taskSearchParameters = new SearchParameters
            {
                Filter = new SearchFilter
                {
                    Value = SearchObjectType.Database,
                },
            };

            var results = await this.client.Search.SearchAsync(
                taskSearchParameters
            );

            var taskDbRes = results.Results.First();

            return taskDbRes.Id;
        }

        /// <summary>
        /// Get the tasks from the database.
        /// </summary>
        /// <param name="startCursor">A Notion provided StartCursor, if any.</param>
        /// <returns>A list of tasks and the cursor to fetch the next batch.</returns>
        public async Task<(List<TodoTask> Tasks, string NextCursor)> GetTasks(string startCursor = null)
        {
            await this.GetDatabaseIdIfNeeded();

            var queryParameters = new DatabasesQueryParameters();

            if (startCursor != null)
            {
                queryParameters.StartCursor = startCursor;
            }

            var results = await this.client.Databases.QueryAsync(this.TaskDatabaseId, queryParameters);

            return (
                Tasks: results.Results.Select(page => page.ToPoco()).ToList(),
                NextCursor: results.NextCursor
            );
        }

        /// <summary>
        /// Updates the <see cref="TodoTask"/> in Notion.
        /// </summary>
        /// <param name="tt">The task to update.</param>
        /// <returns>A task representing the update command.</returns>
        public async Task UpdateTodoTask(TodoTask tt)
        {
            await this.GetDatabaseIdIfNeeded();

            if (tt.Id == null)
            {
                throw new System.Exception("TodoTask is not yet created in database. Cannot update.");
            }

            await this.client.Pages.UpdatePropertiesAsync(tt.Id, tt.BuildUpdateCommand());
        }

        /// <summary>
        /// Adds the <see cref="TodoTask"/> in Notion.
        /// </summary>
        /// <param name="tt">The task to add.</param>
        /// <returns>A task representing the add command.</returns>
        public async Task AddTodoTask(TodoTask tt)
        {
            await this.GetDatabaseIdIfNeeded();

            var pageCreateCommand = new PagesCreateParameters
            {
                Parent = new DatabaseParentInput { DatabaseId = this.TaskDatabaseId },
                Properties = tt.BuildUpdateCommand(),
            };

            await this.client.Pages.CreateAsync(pageCreateCommand);
        }

        private async Task GetDatabaseIdIfNeeded()
        {
            if (this.TaskDatabaseId == null)
            {
                this.TaskDatabaseId = await this.GetTaskDatabaseId();
            }
        }
    }
}
