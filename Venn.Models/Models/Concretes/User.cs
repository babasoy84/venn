using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venn.Models.Models.Abstracts;

namespace Venn.Models.Models.Concretes
{
    public class User : Entity, INotifyPropertyChanged
    {
        private string imageSource;

        private string username;


        public string ImageSource
        {
            get { return imageSource; }
            set
            {
                imageSource = value;
                NotifyPropertyChanged("ImageSource");
            }
        }

        public string Email { get; set; }

        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                NotifyPropertyChanged("Username");
            }
        }

        public string Password { get; set; }

        public DateTime DateOfBirth { get; set; }

        public virtual ObservableCollection<Notification> Notifications { get; set; }

        public virtual ObservableCollection<Message> Messages { get; set; }

        public virtual ObservableCollection<Friendship> Contacts { get; set; }


        public event PropertyChangedEventHandler? PropertyChanged;

        public User()
        {
            ImageSource = "http://res.cloudinary.com/dv9hubcxy/image/upload/v1675676184/poaqh7q2qfur3yvfqgrb.jpg";
            Messages = new ObservableCollection<Message>();
            Notifications = new ObservableCollection<Notification>();
            Contacts = new ObservableCollection<Friendship>();
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public override string ToString() => $"#{Id:D4}";
    }
}
