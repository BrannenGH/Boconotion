using System;
using BocoNotion.TodoTaskManager.Page;
using BocoNotion.TodoTaskManager.Persistence;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Serilog;
using Autofac;
using Notion.Client;
using Serilog.Extensions.Logging;

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

            var logger = loggerProvider.GetLogger();
            builder.RegisterInstance(logger).As<ILogger>();
            NotionClientLogging.ConfigureLogger(new SerilogLoggerFactory(logger));

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
