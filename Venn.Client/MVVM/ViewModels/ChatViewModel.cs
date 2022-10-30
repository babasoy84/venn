using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venn.Client.Net;
using Venn.Models.Models.Concretes;

namespace Venn.Client.MVVM.ViewModels
{
    internal class ChatViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public User User { get; set; }

        public ServerHelper Server { get; set; }

        public ObservableCollection<User> Contacts { get; set; }

        public ChatViewModel()
        {
            User = App.Container.GetInstance<User>();
            Server = App.Container.GetInstance<ServerHelper>();
            Contacts = new ObservableCollection<User>();
            Contacts.Add(new User()
            {
                Username = "babasoy84"
            });
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
