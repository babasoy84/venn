using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Venn.Client.Net;
using Venn.Client.Services;
using Venn.Models.Models.Concretes;

namespace Venn.Client.MVVM.ViewModels
{
    internal class LoginViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private JsonSerializerOptions options = new()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            WriteIndented = true
        };

        private string email;

        private string emailErrorText;

        private string passwordErrorText;

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

        public SnackbarMessageQueue SnackbarMessageQueue { set; get; } = new(TimeSpan.FromSeconds(1));

        public ServerHelper Server { get; set; }

        public RelayCommand<object> ToWelcomeViewCommand { get; set; }

        public RelayCommand<object> ToSignupViewCommand { get; set; }

        public RelayCommand ToForgotPasswordViewCommand { get; set; }

        public RelayCommand<object> LoginCommand { get; set; }

        public LoginViewModel(INavigationService NavigationService, ServerHelper Server)
        {
            this.NavigationService = NavigationService;
            this.Server = Server;

            ToWelcomeViewCommand = new RelayCommand<object>(ToWelcomeView);
            ToSignupViewCommand = new RelayCommand<object>(ToSignupView);
            ToForgotPasswordViewCommand = new RelayCommand(ToForgotPasswordView);
            LoginCommand = new RelayCommand<object>(o => Task.Run(() => Login(o)));
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void ToWelcomeView(object p)
        {
            Email = "";
            EmailErrorText = null;
            PasswordErrorText = null;
            (p as PasswordBox).Password = null;
            NavigationService.NavigateTo<WelcomeViewModel>();
        }

        public void ToSignupView(object p)
        {
            Email = "";
            EmailErrorText = null;
            PasswordErrorText = null;
            (p as PasswordBox).Password = null;
            NavigationService.NavigateTo<CreateAccountViewModel>();
        }

        public void ToForgotPasswordView()
        {
            NavigationService.NavigateTo<ForgotPasswordViewModel>();
        }

        public async void Login(object p)
        {
            string Password = "";

            Application.Current.Dispatcher.Invoke(() => { Password = (p as PasswordBox).Password; });

            if (!new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$").IsMatch(Email))
            {
                EmailErrorText = "Email is not valid!";
            }
            else
            {
                NavigationService.NavigateTo<LoadingViewModel>();
                await Server.Login(Email, Password).ContinueWith((res) =>
                {
                    var str = res.Result;
                    var errorText = str.Split('$')[1];

                    if (str.Split('$')[0] == "email")
                    {
                        NavigationService.NavigateTo<LoginViewModel>();
                        PasswordErrorText = null;
                        EmailErrorText = errorText;
                    }
                    else if (str.Split('$')[0] == "password")
                    {
                        NavigationService.NavigateTo<LoginViewModel>();
                        EmailErrorText = null;
                        PasswordErrorText = errorText;
                    }
                    else
                    {
                        EmailErrorText = null;
                        PasswordErrorText = null;
                        Email = "";
                        Application.Current.Dispatcher.Invoke(() => { (p as PasswordBox).Password = null; });

                        var user = JsonSerializer.Deserialize<User>(str.Split('$')[1], options);
                        App.Container.GetInstance<User>().Id = user.Id;
                        App.Container.GetInstance<User>().ImageSource = user.ImageSource;
                        App.Container.GetInstance<User>().Email = user.Email;
                        App.Container.GetInstance<User>().Username = user.Username;
                        App.Container.GetInstance<User>().Password = user.Password;
                        App.Container.GetInstance<User>().DateOfBirth = user.DateOfBirth;
                        App.Container.GetInstance<User>().Contacts = user.Contacts;
                        App.Container.GetInstance<User>().Messages = user.Messages;
                        App.Container.GetInstance<User>().Notifications = user.Notifications;


                        App.Container.GetInstance<ChatViewModel>().mainEvent.Set();
                        NavigationService.NavigateTo<ChatViewModel>();
                    }
                });
            }
        }

        public void SnackbarEnqueue(string msg, string btnContent = "", Action btnAction = null, double duration = 1)
        {
            SnackbarMessageQueue.Enqueue(msg,
            btnContent,
            _ => btnAction?.Invoke(), actionArgument: null,
            promote: false, neverConsiderToBeDuplicate: false,
            durationOverride: TimeSpan.FromSeconds(duration));
        }
    }
}
