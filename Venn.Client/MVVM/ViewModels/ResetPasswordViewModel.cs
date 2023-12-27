using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using Venn.Client.MVVM.Views;
using Venn.Client.Net;
using Venn.Client.Services;

namespace Venn.Client.MVVM.ViewModels
{
    public class ResetPasswordViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Email { get; set; }


        private string passwordErrorText;

        public string PasswordErrorText
        {
            get { return passwordErrorText; }
            set
            {
                passwordErrorText = value;
                NotifyPropertyChanged("PasswordErrorText");
            }
        }

        public INavigationService NavigationService { get; set; }

        public ServerHelper Server { get; set; }

        public RelayCommand ToBackCommand { get; set; }

        public RelayCommand<object> ResetPasswordCommand { get; set; }

        public ResetPasswordViewModel(INavigationService NavigationService, ServerHelper Server)
        {
            this.Server = Server;
            this.NavigationService = NavigationService;

            ToBackCommand = new RelayCommand(ToBack);
            ResetPasswordCommand = new RelayCommand<object>(ResetPassword);
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void ToBack()
        {
            NavigationService.NavigateTo<ForgotPasswordViewModel>();
        }

        private async void ResetPassword(object p)
        {
            var Password = (p as PasswordBox).Password;

            if (string.IsNullOrWhiteSpace(Password) || !new Regex(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?!.* ).{8,16}$").IsMatch(Password))
                PasswordErrorText = "Password is not valid!";
            else
            {
                PasswordErrorText = "";
                NavigationService.NavigateTo<LoadingViewModel>();
                if (await Server.ResetPassword(Email, Password))
                {
                    NavigationService.NavigateTo<LoginViewModel>();
                    App.Container.GetInstance<LoginViewModel>().SnackbarEnqueue("Your password has changed succesfully.");
                }
            }
        }
    }
}
