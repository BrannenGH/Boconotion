﻿namespace BocoNotion.TodoTaskManager.ViewModel
{
    using System;
    using System.Windows.Input;
    using BocoNotion.TodoTaskManager.Persistence;
    using Microsoft.Toolkit.Mvvm.ComponentModel;
    using Microsoft.Toolkit.Mvvm.Input;

    public class NotionLoginViewModel: ObservableObject
    {
        private string token;
        private ITokenProvider tokenProvider;

        public string Token
        {
            get => this.token;
            set
            {
                this.SetProperty(ref this.token, value);
            }
        }

        public ICommand SaveTokenCommand { get; }
        public ICommand LoadTokenCommand { get; }

        public NotionLoginViewModel(ITokenProvider provider)
        {
            this.tokenProvider = provider;
            this.LoadTokenCommand = new AsyncRelayCommand(async () => this.Token = await tokenProvider.GetToken());
            this.SaveTokenCommand = new AsyncRelayCommand(async () => await this.tokenProvider.SetToken(Token));
        }
    }
}
