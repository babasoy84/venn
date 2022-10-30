using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
    internal class CreateTeamViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string email;

        private string username;

        private string month;

        private int day;

        private int year;

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

        public List<string> Months { get; set; }

        public List<int> Days { get; set; }

        public List<int> Years { get; set; }

        public INavigationService NavigationService { get; set; }

        public ServerHelper Server { get; set; }

        public RelayCommand ToWelcomeViewCommand { get; set; }

        public RelayCommand<object> CreateCommand { get; set; }

        public CreateTeamViewModel(INavigationService NavigationService, ServerHelper Server)
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

            if (!new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$").IsMatch(Email) || 
                !new Regex(@"^[a-zA-Z0-9._]{3,15}$").IsMatch(Username))
            {
                b = false;
            }

            if (b)
            {
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
            }
        }
    }
}
