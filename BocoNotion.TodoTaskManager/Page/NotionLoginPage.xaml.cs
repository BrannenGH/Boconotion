using System;
using System.Collections.Generic;
using BocoNotion.TodoTaskManager.Persistence;
using BocoNotion.TodoTaskManager.ViewModel;
using Xamarin.Forms;

namespace BocoNotion.TodoTaskManager.Page
{
    public partial class NotionLoginPage : ContentPage
    {
        public NavigationPage ParentNavigationPage { get; set; }

        public NotionLoginPage(ITokenProvider provider)
        {
            this.InitializeComponent();
            this.BindingContext = new NotionLoginViewModel(provider);
        }

        public NotionLoginViewModel ViewModel => (NotionLoginViewModel)this.BindingContext;

        async void NotionTokenSubmit_Clicked(Object sender, System.EventArgs e)
        {
            var taskPage = new TodoTaskPage(ViewModel.Token);
            taskPage.ParentNavigationPage = this.ParentNavigationPage;
            await this.ParentNavigationPage.Navigation.PushAsync(taskPage);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}
