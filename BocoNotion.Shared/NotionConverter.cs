namespace BocoNotion.NotionIntegration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Notion.Client;

    /// <summary>
    /// Converts a <see cref="Page"/> to the respective type.
    /// </summary>
    /// <typeparam name="T">
    /// The type Notion is converting to.
    /// </typeparam>
    public abstract class NotionConverter<T>
    {
        /// <summary>
        /// Convert from a <see cref="Page"/> to a <see cref="T"/>.
        /// </summary>
        /// <param name="page">
        /// The Notion page to convert from.
        /// </param>
        /// <returns>
        /// A instance of <see cref="T"/>.
        /// </returns>
        public abstract T Convert(Page page);

        /// <summary>
        /// Convert from a <see cref="T"/> to a <see cref="Page"/>.
        /// </summary>
        /// <param name="poco">
        /// The object to convert from.
        /// </param>
        /// <returns>
        /// A instance of <see cref="Page"/>.
        /// </returns>
        public abstract Page Convert(T poco);

        /// <summary>
        /// Convert a <see cref="TitlePropertyValue"/> to a string.
        /// </summary>
        /// <param name="title">
        /// The property value from Notion.
        /// </param>
        /// <returns>
        /// A string representation of the property.
        /// </returns>
        protected string Convert(TitlePropertyValue title)
        {
            return title?.Title.FirstOrDefault()?.PlainText ?? string.Empty;
        }

        /// <summary>
        /// Convert a <see cref="string"/> to a <see cref="TitlePropertyValue"/>.
        /// </summary>
        /// <param name="title">
        /// The property value from Notion.
        /// </param>
        /// <returns>
        /// A string representation of the property.
        /// </returns>
        protected TitlePropertyValue ConvertToTitle(string title)
        {
            return new TitlePropertyValue()
            {
                Title = new List<RichTextBase>
                {
                    new RichTextBase
                    {
                        PlainText = title,
                    },
                },
            };
        }

        /// <summary>
        /// Convert a <see cref="SelectPropertyValue"/> to a <see cref="string"/>.
        /// </summary>
        /// <param name="select">
        /// The Notion proprety representing the selector.
        /// </param>
        /// <returns>
        /// A string representation fo the <see cref="SelectPropertyValue"/>.
        /// </returns>
        protected string Convert(SelectPropertyValue select)
        {
            return select?.Select?.Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected SelectPropertyValue ConvertToSelect(string name)
        {
            return new SelectPropertyValue()
            {
                Select = new SelectOption()
                {
                    Name = name,
                },
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        protected ICollection<string> Convert(MultiSelectPropertyValue values)
        {
            return values.MultiSelect.Select(x => x.Name).ToArray();
        }

        protected MultiSelectPropertyValue Convert(ICollection<string> values)
        {
            return new MultiSelectPropertyValue()
            {
                MultiSelect = values.Select(value =>
                    new SelectOption()
                    {
                        Name = value,
                    }).ToList() ?? new List<SelectOption>(),
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dueDate"></param>
        /// <returns></returns>
        protected DateTime? Convert(DatePropertyValue dueDate)
        {
            return dueDate?.Date?.End ?? dueDate?.Date?.Start;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        protected DatePropertyValue Convert(DateTime? dateTime)
        {
            if (!dateTime.HasValue)
            {
                return null;
            }

            return new DatePropertyValue
            {
                Date = new Date
                {
                    Start = dateTime,
                },
            };
        }
    }
}
