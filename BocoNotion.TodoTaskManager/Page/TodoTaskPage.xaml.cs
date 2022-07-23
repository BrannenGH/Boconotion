namespace BocoNotion.TodoTaskManager.Page
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Autofac;
    using BocoNotion.TodoTaskManager.ViewModel;
    using Xamarin.Forms;

    public partial class TodoTaskPage : ContentPage
    {
        public NavigationPage ParentNavigationPage { get; set; }

        public TodoTaskPage(IContainer container)
        {
            this.InitializeComponent();

            this.BindingContext = new TodoTasksViewModel();
            container.InjectUnsetProperties(this.BindingContext);
        }

        private TodoTasksViewModel ViewModel => (TodoTasksViewModel)this.BindingContext;

        private async void TodoTaskViewCell_OnClick(Object sender, System.EventArgs e)
        {
            TodoTaskViewModel ttvm = (sender as BindableObject).BindingContext as TodoTaskViewModel;

            await this.ParentNavigationPage.PushAsync(new ContentPage
            {
                Content = new WebView
                {
                    Source = ttvm.TodoTask.NotionUri,
                },
            });
        }
    }
}
