namespace BrannenNotion.Shared.Page
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BocoNotion.Shared.Model;
    using Notion.Client;

    public static class TodoTaskExtensions
    {

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

        public static Dictionary<string, PropertyValue> BuildUpdateCommand(this TodoTask tt)
        {
            return new Dictionary<string, PropertyValue>
            {
                {"Name", tt.GetTitleForNotion() },
                {"State", tt.GetStateForNotion() },
            };
        }

        public static TodoTask ToPoco(this Page page)
        {
            var tt = new TodoTask();
            tt.SetTitleFromNotion(page.Properties["Name"] as TitlePropertyValue);
            tt.SetStateFromNotion(page.Properties["State"] as SelectPropertyValue);
            return tt;
        }
    }
}
