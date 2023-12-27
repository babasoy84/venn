using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using Venn.Client.Net;
using Venn.Client.Services;

namespace Venn.Client.MVVM.ViewModels
{
    class ForgotPasswordViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string email = "";

        private string emailErrorText;

        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                NotifyPropertyChanged("Email");
            }
        }

        public string EmailErrorText
        {
            get { return emailErrorText; }
            set
            {
                emailErrorText = value;
                NotifyPropertyChanged("EmailErrorText");
            }
        }

        public INavigationService NavigationService { get; set; }

        public ServerHelper Server { get; set; }

        public RelayCommand ToLoginViewCommand { get; set; }

        public RelayCommand ToNextViewCommand { get; set; }

        public ForgotPasswordViewModel(INavigationService NavigationService, ServerHelper Server)
        {
            this.Server = Server;
            this.NavigationService = NavigationService;

            ToLoginViewCommand = new RelayCommand(ToLoginView);
            ToNextViewCommand = new RelayCommand(ToNextView);
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void ToLoginView()
        {
            NavigationService.NavigateTo<LoginViewModel>();
        }

        public async void ToNextView()
        {
            if ((!new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$").IsMatch(Email)) || string.IsNullOrWhiteSpace(Email))
            {
                EmailErrorText = "Email is not valid!";
            }
            else
            {
                NavigationService.NavigateTo<LoadingViewModel>();

                if (await Server.CheckEmail(Email))
                {
                    NavigationService.NavigateTo<EmailVerificationViewModel>();
                    App.Container.GetInstance<EmailVerificationViewModel>().Type = "Reset Password";
                    App.Container.GetInstance<EmailVerificationViewModel>().Email = Email;
                    Task.Run(() => App.Container.GetInstance<EmailVerificationViewModel>().SendVertificationCode());
                    EmailErrorText = "";
                    Email = "";
                }
                else
                {
                    Email = "";
                    NavigationService.NavigateTo<ForgotPasswordViewModel>();
                    EmailErrorText = "The email you entered isn’t connected to an account";
                }
            }
        }
    }
}
