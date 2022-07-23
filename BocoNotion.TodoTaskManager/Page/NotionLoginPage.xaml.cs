using System;
using System.Collections.Generic;
using BocoNotion.TodoTaskManager.Persistence;
using BocoNotion.TodoTaskManager.ViewModel;
using Xamarin.Forms;
using Autofac;

namespace BocoNotion.TodoTaskManager.Page
{
    public partial class NotionLoginPage : ContentPage
    {
        public NavigationPage ParentNavigationPage { get; set; }
        private IContainer container;

        public NotionLoginPage(IContainer container)
        {
            this.container = container;
            this.InitializeComponent();
            this.BindingContext = new NotionLoginViewModel();
            container.InjectUnsetProperties(this.BindingContext);
        }

        public NotionLoginViewModel ViewModel => (NotionLoginViewModel)this.BindingContext;

        async void NotionTokenSubmit_Clicked(Object sender, System.EventArgs e)
        {
            var taskPage = new TodoTaskPage(this.container);
            taskPage.ParentNavigationPage = this.ParentNavigationPage;
            await this.ParentNavigationPage.Navigation.PushAsync(taskPage);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}
