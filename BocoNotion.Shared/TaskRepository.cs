namespace BocoNotion.Shared
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
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
        /// The Notion ID for the task database.
        /// </summary>
        public string TaskDatabaseId { get; set; }

        private async Task GetDatabaseIdIfNeeded()
        {
            if (TaskDatabaseId == null)
            {
                TaskDatabaseId = await GetTaskDatabaseId();
            }
        } 

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

            var results = await client.Search.SearchAsync(
                taskSearchParameters
            );

            var taskDbRes = results.Results.First();

            return taskDbRes.Id;
        }

        public async Task<PaginatedList<Page>> GetTasks()
        {
            await this.GetDatabaseIdIfNeeded();

            return await client.Databases.QueryAsync(this.TaskDatabaseId, new DatabasesQueryParameters());
        }

        public async Task UpdateTodoTask(
            string todoTaskId,
            string name,
            bool isCompleted
        )
        {
            var updateProperties = new Dictionary<string, PropertyValue>
            {
                {
                    "Name",
                    new TitlePropertyValue()
                        { Title = new List<RichTextBase>() { new RichTextText() { Text = new Text { Content = name } } } }
                },
                {
                    "State",
                    new SelectPropertyValue() { Select = new SelectOption() { Name = isCompleted ? "Done" : "Not Ready" } }
                },
            };
            await client.Pages.UpdatePropertiesAsync(todoTaskId, updateProperties);
        }
    }
}
