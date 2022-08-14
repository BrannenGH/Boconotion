using System;
using System.Collections.Generic;
using BocoNotion.TodoTaskManager.ViewModel;
using Xamarin.Forms;

namespace BocoNotion.TodoTaskManager.Page
{
    public partial class FilterTodoTaskPage : ContentPage
    {
        public FilterTodoTaskPage()
        {
            InitializeComponent();
        }

        void Button_Pressed(System.Object sender, System.EventArgs e)
        {
            (this.BindingContext as TodoTasksViewModel).FilterTagName = (sender as Button).Text;
        }
    }
}

