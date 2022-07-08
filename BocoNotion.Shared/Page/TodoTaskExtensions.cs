namespace BrannenNotion.Shared.Page
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BocoNotion.Shared.Model;
    using Notion.Client;

    public static class TodoTaskExtensions
    {
        #region Title
        public static TodoTask SetTitleFromNotion(this TodoTask tt, TitlePropertyValue title)
        {
            tt.Title = title?.Title.First()?.PlainText;
            return tt;
        }

        public static TitlePropertyValue GetTitleForNotion(this TodoTask tt)
        {
            return new TitlePropertyValue()
            {
                Title = new List<RichTextBase>()
                {
                    new RichTextText()
                    {
                        Text = new Text { Content = tt.Title },
                    },
                },
            };
        }
        #endregion

        #region State
        public static TodoTask SetStateFromNotion(this TodoTask tt, SelectPropertyValue state)
        {
            tt.Checked = state?.Select?.Name == "Done";
            return tt;
        }

        public static SelectPropertyValue GetStateForNotion(this TodoTask tt)
        {
            return new SelectPropertyValue()
            {
                Select = new SelectOption()
                {
                    Name = tt.Checked ? "Done" : "Not Ready",
                },
            };
        }
        #endregion

        #region Tags
        public static TodoTask SetTagsFromNotion(this TodoTask tt, MultiSelectPropertyValue tags)
        {
            tt.Tags = tags.MultiSelect.Select(x => x.Name).ToArray();
            return tt;
        }

        public static MultiSelectPropertyValue GetTagForNotion(this TodoTask tt)
        {
            return new MultiSelectPropertyValue()
            {
                MultiSelect = tt.Tags.Select(tag =>
                    new SelectOption()
                    {
                        Name = tag
                    }).ToList(),
            };
        }
        #endregion

        #region Due Date

        public static TodoTask SetDueDateFromNotion(this TodoTask tt, DatePropertyValue dueDate)
        {
            tt.DueDate = dueDate?.Date?.End ?? dueDate?.Date?.Start;
            return tt;
        }

        public static DatePropertyValue GetDueDateForNotion(this TodoTask tt)
        {
            return new DatePropertyValue
            {
                Date = new Date
                {
                    End = tt.DueDate,
                },
            };
        }
        #endregion

        public static Dictionary<string, PropertyValue> BuildUpdateCommand(this TodoTask tt)
        {
            return new Dictionary<string, PropertyValue>
            {
                { "Name", tt.GetTitleForNotion() },
                { "State", tt.GetStateForNotion() },
                { "Tags", tt.GetTagForNotion() },
                { "Due Date", tt.GetDueDateForNotion() },
            };
        }

        public static TodoTask ToPoco(this Page page)
        {
            var tt = new TodoTask();
            tt.SetTitleFromNotion(page.Properties["Name"] as TitlePropertyValue);
            tt.SetStateFromNotion(page.Properties["State"] as SelectPropertyValue);
            tt.SetTagsFromNotion(page.Properties["Tags"] as MultiSelectPropertyValue);
            tt.SetDueDateFromNotion(page.Properties["Due Date"] as DatePropertyValue);
            return tt;
        }
    }
}
