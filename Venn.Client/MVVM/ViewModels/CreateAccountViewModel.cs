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

        public List<string> Months { get; set; }

        public List<int> Days { get; set; }

        public List<int> Years { get; set; }

        public INavigationService NavigationService { get; set; }

        public ServerHelper Server { get; set; }

        public RelayCommand ToWelcomeViewCommand { get; set; }

        public RelayCommand<object> CreateCommand { get; set; }

        public CreateAccountViewModel(INavigationService NavigationService, ServerHelper Server)
        {
            this.Server = Server;
            this.NavigationService = NavigationService;

            ToWelcomeViewCommand = new RelayCommand(ToWelcomeView);
            CreateCommand = new RelayCommand<object>(Create);

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
            for (int i = 1870; i <= DateTime.Now.Year - 3; i++)
            {
                Years.Add(i);
            }
        }

        public void ToWelcomeView()
        {
            NavigationService.NavigateTo<WelcomeViewModel>();
        }

        private void Create(object p)
        {
            var Password = (p as PasswordBox).Password;
            var b = true;

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

            if (b)
            {
                try
                {
                    MailAddress mailAddress = new MailAddress(Email);

                    try
                    {
                        string verificationToken = Guid.NewGuid().ToString();

                        using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                        {
                            smtpClient.Host = "smtp.example.com";
                            smtpClient.Port = 587;
                            smtpClient.EnableSsl = true;
                            smtpClient.Credentials = new NetworkCredential("dma.venn@gmail.com", "Venn2023");

                            MailMessage message = new MailMessage();
                            message.From = new MailAddress("dma.venn@gmail.com");
                            message.To.Add(Email);
                            message.Subject = "Email Verification";
                            message.Body = $"Your verification token is: {verificationToken}\nPlease reply to this email to verify your email address.";
                        }

                        var dt = new DateTime(Year, Months.FindIndex(m => m == Month) + 1, Day);
                        var user = new User();
                        user.Email = Email;
                        user.Username = Username;
                        user.Password = Password;
                        user.DateOfBirth = dt;
                        if (Server.CreateTeam(user))
                        {
                            Email = null;
                            Username = null;
                            (p as PasswordBox).Password = null;
                            Day = 0;
                            Year = 0;
                            Month = null;
                            NavigationService.NavigateTo<WelcomeViewModel>();
                        }
                        else
                        {
                            EmailErrorText = "This email has been used, email must be unique!";
                        }
                    }
                    catch (SmtpException ex)
                    {
                        throw;
                    }
                }
                catch (FormatException)
                {
                    EmailErrorText = "No such email!";
                }
            }
        }
    }
}
