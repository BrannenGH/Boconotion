using System;
using BocoNotion.TodoTaskManager.Page;
using BocoNotion.TodoTaskManager.Persistence;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BocoNotion.TodoTaskManager
{
    public partial class App : Application
    {
        public App(ITokenProvider provider)
        {
            this.InitializeComponent();

            var firstPage = new NotionLoginPage(provider);

            var navigation = new NavigationPage(firstPage);

            firstPage.ParentNavigationPage = navigation;

            MainPage = navigation;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
