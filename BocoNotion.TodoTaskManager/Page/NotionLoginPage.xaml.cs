using System;
using System.Collections.Generic;
using BocoNotion.TodoTaskManager.ViewModel;
using Xamarin.Forms;

namespace BocoNotion.TodoTaskManager.Page
{
    public partial class NotionLoginPage : ContentPage
    {
        public INavigation NavigationManager { get; set; }

        public NotionLoginPage()
        {
            this.InitializeComponent();

            this.BindingContext = new NotionLoginViewModel();
        }

        public NotionLoginViewModel ViewModel => (NotionLoginViewModel)this.BindingContext;

        async void NotionTokenSubmit_Clicked(Object sender, System.EventArgs e)
        {
            var taskPage = new TodoTaskPage(ViewModel.Token);
            taskPage.NavigationManager = this.NavigationManager;
            await this.NavigationManager.PushAsync(taskPage);
        }
    }
}
