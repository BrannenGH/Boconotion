﻿namespace BocoNotion.TodoTaskManager.Page
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BocoNotion.TodoTaskManager.ViewModel;
    using Xamarin.Forms;

    public partial class TodoTaskPage : ContentPage
    {
        public NavigationPage ParentNavigationPage { get; set; }

        public TodoTaskPage(string token)
        {
            this.InitializeComponent();

            this.BindingContext = new TodoTasksViewModel(token);
        }

        private TodoTasksViewModel ViewModel => (TodoTasksViewModel)this.BindingContext;
    }
}
