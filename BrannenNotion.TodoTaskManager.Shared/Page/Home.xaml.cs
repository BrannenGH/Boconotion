namespace BrannenNotion.TodoTaskManager.Shared.Page
{
    using System.ComponentModel;
    using BrannenNotion.TodoTaskManager.ViewModel;
    using Page = Windows.UI.Xaml.Controls.Page;

    /// <summary>
    /// Home page.
    /// </summary>
    public sealed partial class Home : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Home"/> class.
        /// </summary>
        public Home()
        {
            this.InitializeComponent();

            this.DataContext = new TodoTaskViewModel();
        }

        [Bindable(true)]
        private TodoTaskViewModel ViewModel => (TodoTaskViewModel)this.DataContext;
    }
}
