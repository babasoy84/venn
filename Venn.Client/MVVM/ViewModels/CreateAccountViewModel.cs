using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Venn.Client.Net;
using Venn.Client.Services;
using Venn.Models.Models.Concretes;

namespace Venn.Client.MVVM.ViewModels
{
    internal class CreateAccountViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string email;

        private string username;

        private string month;

        private int day;

        private int year;

        private string emailErrorText;

        private string usernameErrorText;

        private string passwordErrorText;

        public User User { get; set; } = new User();

        public string Email
        {
            get { return email; }
            set
            {
                email = value;
            }
        }

        public string Username
        {
            get { return username; }
            set
            {
                username = value;
            }
        }

        public string Month
        {
            get { return month; }
            set
            {
                month = value;
                NotifyPropertyChanged("Month");
            }
        }

        public int Day
        {
            get { return day; }
            set
            {
                day = value;
                NotifyPropertyChanged("Day");
            }
        }

        public int Year
        {
            get { return year; }
            set
            {
                year = value;
                NotifyPropertyChanged("Year");
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

        public string UsernameErrorText
        {
            get { return usernameErrorText; }
            set
            {
                usernameErrorText = value;
                NotifyPropertyChanged("UsernameErrorText");
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

        public string Password { get; set; }

        public List<string> Months { get; set; }

        public List<int> Days { get; set; }

        public List<int> Years { get; set; }

        public INavigationService NavigationService { get; set; }

        public ServerHelper Server { get; set; }

        public RelayCommand ToWelcomeViewCommand { get; set; }

        public RelayCommand ToLoginViewCommand { get; set; }

        public RelayCommand<object> CreateCommand { get; set; }

        public CreateAccountViewModel(INavigationService NavigationService, ServerHelper Server)
        {
            this.Server = Server;
            this.NavigationService = NavigationService;

            ToWelcomeViewCommand = new RelayCommand(ToWelcomeView);
            ToLoginViewCommand = new RelayCommand(ToLoginView);
            CreateCommand = new RelayCommand<object>(o => Task.Run(() => Create(o)));

            InitMonths();
            InitYears();
            InitDays();
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void InitMonths()
        {
            Months = new List<string>() {
                "January", "February", "March",
                "April", "May", "June",
                "July", "August", "September",
                "October", "November", "December"
            };
        }

        public void InitDays()
        {
            Days = new List<int>();
            for (int i = 1; i <= 31; i++)
            {
                Days.Add(i);
            }
        }

        public void InitYears()
        {
            Years = new List<int>();
            for (int i = DateTime.Now.Year - 153; i <= DateTime.Now.Year - 3; i++)
            {
                Years.Add(i);
            }
        }

        public void ToWelcomeView()
        {
            NavigationService.NavigateTo<WelcomeViewModel>();
        }

        public void ToLoginView()
        {
            NavigationService.NavigateTo<LoginViewModel>();
        }

        private async Task<bool> CheckValidate(object p)
        {
            Password = (p as PasswordBox).Password;
            bool b = true;
            if (string.IsNullOrWhiteSpace(Email) || !new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$").IsMatch(Email))
            {
                EmailErrorText = "Email is not valid!";
                b = false;
            }
            else
            {
                EmailErrorText = "";
            }
            if (string.IsNullOrWhiteSpace(Username) || !new Regex(@"^[a-zA-Z0-9._]{3,15}$").IsMatch(Username))
            {
                UsernameErrorText = "Username is not valid!";
                b = false;
            }
            else
            {
                UsernameErrorText = "";
            }
            if (string.IsNullOrWhiteSpace(Password) || !new Regex(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?!.* ).{8,16}$").IsMatch(Password))
            {
                PasswordErrorText = "Password is not valid!";
                b = false;
            }
            else
            {
                PasswordErrorText = "";
            }
            if (Month == null || Day == null || Year == null)
            {
                b = false;
            }

            return b;
        }

        private async Task Create(object p)
        {
            if (await CheckValidate(p))
            {

                NavigationService.NavigateTo<LoadingViewModel>();

                var dt = new DateTime(Year, Months.FindIndex(m => m == Month) + 1, Day);
                User.Email = Email;
                User.Username = Username;
                User.Password = Password;
                User.DateOfBirth = dt;

                if (!await Server.CheckEmail(Email))
                {
                    NavigationService.NavigateTo<EmailVerificationViewModel>();
                    App.Container.GetInstance<EmailVerificationViewModel>().Type = "Create Account";
                    App.Container.GetInstance<EmailVerificationViewModel>().Email = Email;
                    App.Container.GetInstance<EmailVerificationViewModel>().SendVertificationCode();


                    Email = null;
                    Username = null;
                    (p as PasswordBox).Password = null;
                    Day = 0;
                    Year = 0;
                    Month = null;
                }
                else
                {
                    NavigationService.NavigateTo<CreateAccountViewModel>();
                    EmailErrorText = "This email has been used, email must be unique!";
                }
            }
        }

        public async void CreateAccount()
        {
            await Server.CreateAccount(User);
        }
    }
}
