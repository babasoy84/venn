using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Venn.Client.Net;
using Venn.Models.Models.Concretes;

namespace Venn.Client.MVVM.ViewModels
{
    internal class ChatViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public AutoResetEvent mainEvent;

        public User User { get; set; }

        public ServerHelper Server { get; set; }

        public ObservableCollection<User> Contacts { get; set; }

        public ChatViewModel()
        {
            User = App.Container.GetInstance<User>();
            Server = App.Container.GetInstance<ServerHelper>();
            mainEvent = new AutoResetEvent(false);
            Task.Run(() =>
            {
                while (true)
                {
                    mainEvent.WaitOne();
                    var ns = Server.client.GetStream();
                    var bytes = new byte[4096];
                    var length = ns.Read(bytes, 0, bytes.Length);
                    var str = Encoding.Default.GetString(bytes, 0, length);
                    var command = str.Split('$')[0];
                    if (command == "contacts")
                    {
                        Contacts = new ObservableCollection<User>();
                        foreach (var contact in JsonSerializer.Deserialize<ObservableCollection<User>>(str.Split('$')[1]))
                        {
                            Contacts.Add(contact);
                        }
                    }
                }
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
