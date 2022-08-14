using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace BocoNotion.TodoTaskManager.Page
{
    public partial class TodoTaskFlyoutPage : FlyoutPage
    {
        private NavigationPage parentNavigationPage;

        public NavigationPage ParentNavigationPage {
            get => this.parentNavigationPage;
            set
            {
                this.parentNavigationPage = value;
                this.TodoTaskPage.ParentNavigationPage = this.ParentNavigationPage;
            }
        }

        public TodoTaskFlyoutPage()
        {
            InitializeComponent();
        }
    }
}

