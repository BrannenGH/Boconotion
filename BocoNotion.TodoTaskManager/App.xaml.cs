using System;
using BocoNotion.TodoTaskManager.Page;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BocoNotion.TodoTaskManager
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var firstPage = new NotionLoginPage();

            var navigation = new NavigationPage(firstPage);

            firstPage.NavigationManager = navigation.Navigation;

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
