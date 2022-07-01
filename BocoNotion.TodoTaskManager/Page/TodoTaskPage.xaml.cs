using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BocoNotion.TodoTaskManager.ViewModel;
using Xamarin.Forms;

namespace BocoNotion.TodoTaskManager.Page
{
    public partial class TodoTaskPage : ContentPage
    {
        public INavigation NavigationManager { get; set; }

        public TodoTaskPage(string token)
        {
            this.InitializeComponent();

            this.BindingContext = new TodoTaskViewModel(token);

            this.TodoTaskListView.SetBinding(ListView.ItemsSourceProperty, new Binding("."));
            this.TodoTaskListView.BindingContext = ViewModel.TodoTasks;
        }

        private TodoTaskViewModel ViewModel => (TodoTaskViewModel)this.BindingContext;
    }
}
