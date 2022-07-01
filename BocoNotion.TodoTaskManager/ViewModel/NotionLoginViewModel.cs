namespace BocoNotion.TodoTaskManager.ViewModel
{
    using System;
    using System.Windows.Input;
    using Microsoft.Toolkit.Mvvm.ComponentModel;

    public class NotionLoginViewModel: ObservableObject
    {
        private string token;

        public string Token
        {
            get => this.token;
            set
            {
                this.SetProperty(ref this.token, value);
            }
        }

        public ICommand SaveTokenCommand { get; }

        public NotionLoginViewModel()
        {
                
        }
    }
}
