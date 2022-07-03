namespace BocoNotion.TodoTaskManager.Page
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BocoNotion.TodoTaskManager.ViewModel;
    using Xamarin.Forms;
    using BocoNotion.TodoTaskManager.Component;

    public partial class TodoTaskPage : ContentPage
    {
        public NavigationPage ParentNavigationPage { get; set; }

        public TodoTaskPage(string token)
        {
            this.InitializeComponent();

            this.BindingContext = new TodoTasksViewModel(token);

            this.TodoTaskListView.SetBinding(ListView.ItemsSourceProperty, new Binding("."));
            this.TodoTaskListView.BindingContext = ViewModel.TodoTasks;
        }

        private TodoTasksViewModel ViewModel => (TodoTasksViewModel)this.BindingContext;
    }
}
