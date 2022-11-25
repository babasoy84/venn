using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
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

        public ServerHelper Server { get; set; }

        public RelayCommand ToWelcomeViewCommand { get; set; }

        public RelayCommand<object> LoginCommand { get; set; }

        public LoginViewModel(INavigationService NavigationService, ServerHelper Server)
        {
            this.NavigationService = NavigationService;
            this.Server = Server;

            ToWelcomeViewCommand = new RelayCommand(ToWelcomeView);
            LoginCommand = new RelayCommand<object>(Login);
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void ToWelcomeView()
        {
            NavigationService.NavigateTo<WelcomeViewModel>();
        }

        public void Login(object p)
        {
            var Password = (p as PasswordBox).Password;

            var b = true;

            if (!new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$").IsMatch(Email))
            {
                b = false;
            }

            if (b)
            {
                var str = Server.Login(Email, Password);
                var errorText = str.Split('$')[1];
                if (str.Split('$')[0] == "email")
                {
                    PasswordErrorText = null;
                    EmailErrorText = errorText;
                }
                else if(str.Split('$')[0] == "password")
                {
                    EmailErrorText = null;
                    PasswordErrorText = errorText;
                }
                else
                {
                    EmailErrorText = null;
                    PasswordErrorText = null;

                    JsonSerializerOptions options = new()
                    {
                        ReferenceHandler = ReferenceHandler.IgnoreCycles,
                        WriteIndented = true
                    };

                    var user = JsonSerializer.Deserialize<User>(str.Split('$')[1], options);
                    App.Container.GetInstance<User>().Id = user.Id;
                    App.Container.GetInstance<User>().ImageSource = user.ImageSource;
                    App.Container.GetInstance<User>().Email = user.Email;
                    App.Container.GetInstance<User>().Username = user.Username;
                    App.Container.GetInstance<User>().Password = user.Password;
                    App.Container.GetInstance<User>().DateOfBirth = user.DateOfBirth;
                    App.Container.GetInstance<User>().Messages = user.Messages;
                    App.Container.GetInstance<User>().Rooms = user.Rooms;
                    App.Container.GetInstance<ChatViewModel>().mainEvent.Set();
                    NavigationService.NavigateTo<ChatViewModel>();
                }
            }
        }
    }
}
