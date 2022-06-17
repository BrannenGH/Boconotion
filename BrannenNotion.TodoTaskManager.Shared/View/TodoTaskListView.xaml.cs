using Microsoft.Extensions.Configuration;

namespace BrannenNotion.TodoTaskManager.Shared.View
{
    using BrannenNotion.Shared;
    using BrannenNotion.TodoTaskManager.ViewModel;
    using Notion.Client;
    using Windows.UI.Xaml;
    using Page = Windows.UI.Xaml.Controls.Page;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TodoTaskListView : Page
    {
        private readonly TodoTaskViewModel todoTaskViewModel; 

        public TodoTaskListView()
        {
            this.InitializeComponent();

            // TODO: Use inversion of control
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var client = NotionClientFactory.Create(new ClientOptions
            {
                AuthToken = config["NotionSecret"],
            });

            var repository = new TaskRepository(client);

            this.todoTaskViewModel = new TodoTaskViewModel(repository);
        }

        private async void TodoTaskListView_OnLoaded(object sender, RoutedEventArgs e)
        {
            await this.todoTaskViewModel.LoadTasks();
        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            await this.todoTaskViewModel.UpdateTasks();
        }
    }
}
