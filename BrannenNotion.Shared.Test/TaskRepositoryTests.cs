namespace BrannenNotion.Shared.Test
{
    using System;
    using Moq;
    using Notion.Client;
    using Xunit;

    /// <summary>
    /// Tests for the <see cref="TaskRepository"/>.
    /// </summary>
    public class TaskRepositoryTests
     {
         private TaskRepository taskRepository; 

         public TaskRepositoryTests()
         {
             var clientMoq = BuildMoqNotionClient();

             taskRepository = new TaskRepository(clientMoq.Object);
         }

         private Mock<NotionClient> BuildMoqNotionClient()
         {
             var mock = new Mock<NotionClient>(MockBehavior.Strict);

             return mock;
         }

         [Fact]
         public async void TaskRepositoryShouldFetchDatabase()
         {
             var databaseId = await taskRepository.GetTaskDatabaseId();

             Assert.True(!String.IsNullOrWhiteSpace(databaseId));
         }

         [Fact]
         public async void TaskRepositoryShouldFetchTasks()
         {
             var tasks = await taskRepository.GetTasks();

             Assert.NotEmpty(tasks.Results);
         }
     }
}

