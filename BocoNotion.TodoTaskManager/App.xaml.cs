using System;
using BocoNotion.TodoTaskManager.Page;
using BocoNotion.TodoTaskManager.Persistence;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Serilog;
using Autofac;

namespace BocoNotion.TodoTaskManager
{
    public partial class App : Application
    {
        public App(ITokenProvider provider, ILoggerProvider loggerProvider)
        {
            this.InitializeComponent();

            Log.Logger = loggerProvider.GetLogger();

            var builder = new ContainerBuilder();

            builder.RegisterInstance(provider).As<ITokenProvider>();

            builder.RegisterInstance(loggerProvider.GetLogger()).As<ILogger>();

            var container = builder.Build();

            var firstPage = new NotionLoginPage(container);

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
