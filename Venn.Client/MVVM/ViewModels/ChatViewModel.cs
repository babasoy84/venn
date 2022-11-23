using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
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

        private ObservableCollection<User> contacts;

        private ObservableCollection<Message> messages;

        private User selectedContact;

        private JsonSerializerOptions options;

        private string text;


        public ObservableCollection<User> Contacts
        {
            get { return contacts; }
            set
            {
                contacts = value;
                NotifyPropertyChanged("Contacts");
            }
        }

        public ObservableCollection<Message> Messages
        {
            get { return messages; }
            set
            {
                messages = value;
                NotifyPropertyChanged("Messages");
            }
        }

        public User SelectedContact
        {
            get { return selectedContact; }
            set
            {
                selectedContact = value;
                NotifyPropertyChanged("SelectedContact");
                UpdateMessages();
            }
        }

        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                NotifyPropertyChanged("Text");
            }
        }


        public Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

        public RelayCommand SendMessageCommand { get; set; }

        public ChatViewModel()
        {
            options = new()
            {
                NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };
            User = App.Container.GetInstance<User>();
            Server = App.Container.GetInstance<ServerHelper>();
            Contacts = new ObservableCollection<User>();
            Messages = new ObservableCollection<Message>();
            SendMessageCommand = new RelayCommand(SendMessage);
            mainEvent = new AutoResetEvent(false);
            Task.Run(() =>
            {
                mainEvent.WaitOne();
                while (true)
                {
                    var ns = Server.client.GetStream();
                    var bytes = new byte[4096];
                    var length = ns.Read(bytes, 0, bytes.Length);
                    var str = Encoding.Default.GetString(bytes, 0, length);
                    var command = str.Split('$')[0];
                    if (command == "contacts")
                    {
                        lock (this)
                        {
                            dispatcher.Invoke(() => Contacts.Clear());
                            foreach (var contact in JsonSerializer.Deserialize<ObservableCollection<User>>(str.Split('$')[1], options))
                            {
                                dispatcher.Invoke(() => Contacts.Add(contact));
                            }
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

        public void UpdateMessages()
        {
            if (SelectedContact != null)
            {
                var lst = new List<Message>();
                foreach (var msg in User.Messages.Where(m => m.ToUserId == SelectedContact.Id))
                {
                    lst.Add(msg);
                }

                foreach (var msg in SelectedContact.Messages.Where(m => m.ToUserId == User.Id))
                {
                    lst.Add(msg);
                }

                Messages.Clear();
                foreach (var msg in lst.OrderBy(m => m.SendingTime).ToList())
                {
                    Messages.Add(msg);
                }
            }
        }

        public void SendMessage()
        {
            var Message = new Message();
            Message.MessageType = "text";
            Message.FromUserId = User.Id;
            Message.ToUserId = SelectedContact.Id;
            Message.Data = Encoding.UTF8.GetBytes(Text);
            Message.SendingTime = DateTime.Now;

            User.Messages.Add(Message);

            var str = $"message${JsonSerializer.Serialize(Message)}";

            Server.client.Client.Send(Encoding.UTF8.GetBytes(str));

            UpdateMessages();
        }
    }
}
