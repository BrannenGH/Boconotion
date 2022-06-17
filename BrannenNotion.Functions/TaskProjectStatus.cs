using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Notion.Client;

namespace BrannenNotion.Functions;

/// <summary>
///     Azure functions to update the task/project status for a
/// </summary>
public static class TaskProjectStatus
{
    [FunctionName("TaskProjectStatus")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req,
        ILogger log)
    {
        try
        {
            log.Log(LogLevel.Information, "Running function");
            var client = NotionClientFactory.Create(new ClientOptions
            {
                AuthToken = Environment.GetEnvironmentVariable("NotionSecret")
            });

            var taskSearchParameters = new SearchParameters
            {
                Filter = new SearchFilter
                {
                    Value = SearchObjectType.Database
                }
            };

            var results = await client.Search.SearchAsync(
                taskSearchParameters
            );

            var taskDbRes = results.Results.First();
            var taskDb = await client.Databases.RetrieveAsync(taskDbRes.Id);

            var tasksWithoutStatus = await client.Databases.QueryAsync(taskDbRes.Id, new DatabasesQueryParameters
            {
                Filter = new SelectFilter("State", isNotEmpty: true)
            });

            foreach (var task in tasksWithoutStatus.Results)
                await client.Pages.UpdatePropertiesAsync(task.Id, new Dictionary<string, PropertyValue>
                {
                    /*{ "State", new SelectPropertyValue { Select = new SelectOption { Name = "Not Ready" } } }*/
                });

            return new OkObjectResult("Processed");
        }
        catch (Exception e)
        {
            ;
        }

        return new OkObjectResult("Error");
    }
}