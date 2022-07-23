namespace BocoNotion.NotionIntegration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BocoNotion.Model;
    using BocoNotion.NotionIntegration.NotionConverter;
    using Notion.Client;
    using Serilog;
    using Serilog.Core;

    /// <summary>
    /// Repository to proxy calls to manage the Notion task database.
    /// </summary>
    public class TaskRepository
    {
        private readonly NotionClient client;
        private readonly TodoTaskConverter todoTaskConverter = new TodoTaskConverter();

        public ILogger Logger { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskRepository"/> class.
        /// </summary>
        /// <param name="client">The authenticated Notion Client.</param>
        /// <param name="taskDatabaseId">Optional database ID for tasks.</param>
        public TaskRepository(
            NotionClient client,
            string taskDatabaseId = null
        )
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

            this.Logger?.Debug("Fetching database ID {@TaskSearchParameters}", taskSearchParameters);
            try
            {
                var results = await this.client.Search.SearchAsync(
                    taskSearchParameters
                );

                var taskDbRes = results.Results.First();

                this.Logger?.Debug("Got back database results {@Results}. Task database is {@TaskDbRes}", results, taskDbRes);

                return taskDbRes.Id;

            }
            catch (Exception e)
            {
                this.Logger?.Error(e, "Failed to get database from Notion.");

                // Throw error after it is logged.
                throw e;
            }
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

            try
            {
                var results = await this.client.Databases.QueryAsync(this.TaskDatabaseId, queryParameters);

                var convertedTasks = results.Results.Select(page => this.todoTaskConverter.Convert(page)).ToList();

                this.Logger.Debug("Converted tasks {@ConvertedTasks}", convertedTasks);

                return (
                    Tasks: convertedTasks,
                    NextCursor: results.NextCursor
                );
            }
            catch (Exception e)
            {
                this.Logger.Error(e, "Failed to get tasks from Notion.");

                // Throw error after it is logged.
                throw e;
            }

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

            try
            {
                await this.client.Pages.UpdatePropertiesAsync(tt.Id, this.todoTaskConverter.Convert(tt).Properties);
            }
            catch (Exception e)
            {
                this.Logger?.Error(e, "Could not update TodoTask");

                throw e;
            }
        }

        /// <summary>
        /// Adds the <see cref="TodoTask"/> in Notion.
        /// </summary>
        /// <param name="tt">The task to add.</param>
        /// <returns>A task representing the add command.</returns>
        public async Task AddTodoTask(TodoTask tt)
        {
            await this.GetDatabaseIdIfNeeded();

            try
            {
                var pageCreateCommand = new PagesCreateParameters
                {
                    Parent = new DatabaseParentInput { DatabaseId = this.TaskDatabaseId },
                    Properties = this.todoTaskConverter.Convert(tt).Properties,
                };

                await this.client.Pages.CreateAsync(pageCreateCommand);
            }
            catch (Exception e)
            {
                this.Logger?.Error(e, "Could not add TodoTask");
                throw e;
            }
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
