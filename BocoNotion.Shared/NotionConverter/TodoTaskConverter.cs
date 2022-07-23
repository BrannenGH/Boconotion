namespace BocoNotion.NotionIntegration.NotionConverter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BocoNotion.Model;
    using BocoNotion.NotionIntegration;
    using Notion.Client;

    public class TodoTaskConverter: NotionConverter<TodoTask>
    {
        public TodoTaskConverter()
        {
        }

        /// <inheritdoc/>
        public override TodoTask Convert(Page page)
        {
            var tt = new TodoTask();
            tt.Id = page.Id;
            tt.Title = this.Convert(page.Properties["Name"] as TitlePropertyValue);
            tt.Checked = this.Convert(page.Properties["State"] as SelectPropertyValue) == "Done";
            tt.Tags = this.Convert(page.Properties["Tags"] as MultiSelectPropertyValue).ToArray();
            tt.DueDate = this.Convert(page.Properties["Due Date"] as DatePropertyValue);
            tt.NotionUri = new Uri(page.Url);
            return tt;
        }

        /// <inheritdoc/>
        public override Page Convert(TodoTask poco)
        {
            return new Page
            {
                Id = string.Empty,
                Properties = new Dictionary<string, PropertyValue>
                {
                    { "Name", this.ConvertToTitle(poco.Title) },
                    { "State", this.ConvertToSelect(poco.Checked ? "Done" : "Ready to Execute") },
                    { "Tags", this.Convert(poco.Tags) },
                    { "Due Date", this.Convert(poco.DueDate) },
                },
            };
        }
    }
}
