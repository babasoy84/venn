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

        private ObservableCollection<User> users;

        private ObservableCollection<Message> messages;

        private User selectedContact;

        private int selectedContactIndex;

        private JsonSerializerOptions options;

        private string text;

        private string friendName;


        public ObservableCollection<User> Contacts
        {
            get { return contacts; }
            set
            {
                contacts = value;
                NotifyPropertyChanged("Contacts");
            }
        }

        public ObservableCollection<User> Users
        {
            get { return users; }
            set
            {
                users = value;
                NotifyPropertyChanged("Users");
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

        public int SelectedContactIndex
        {
            get { return selectedContactIndex; }
            set
            {
                selectedContactIndex = value;
                NotifyPropertyChanged("SelectedContactIndex");
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

        public string FriendName
        {
            get { return friendName; }
            set
            {
                friendName = value;
                NotifyPropertyChanged("FriendName");
            }
        }


        public Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

        public RelayCommand SendMessageCommand { get; set; }

        public ChatViewModel()
        {
            options = new()
            {
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
                    var bytes = new byte[ushort.MaxValue - 28];
                    var length = ns.Read(bytes, 0, bytes.Length);
                    var str = Encoding.Default.GetString(bytes, 0, length);
                    if (str[0] == '<')
                    {
                        while (true)
                        {
                            ns = Server.client.GetStream();
                            bytes = new byte[ushort.MaxValue - 28];
                            length = ns.Read(bytes, 0, bytes.Length);
                            var s = Encoding.Default.GetString(bytes, 0, length);
                            str += s;
                            if (s.Last() == '>')
                            {
                                str = str.Remove(0, 1);
                                str = str.Remove(str.Length - 1, 1); 
                                break;
                            }
                        }
                    }
                    var command = str.Split('$')[0];
                    if (command == "contacts")
                    {
                        int ind = -1;

                        if (SelectedContactIndex != null)
                            ind = SelectedContactIndex;

                        lock (this)
                        {
                            dispatcher.Invoke(() => Contacts.Clear());
                            foreach (var contact in JsonSerializer.Deserialize<ObservableCollection<User>>(str.Split('$')[1], options))
                            {
                                dispatcher.Invoke(() => Contacts.Add(contact));
                            }
                        }

                        if (ind >= 0 && ind < Contacts.Count())
                            SelectedContactIndex = ind;
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
                    msg.IsSelf = true;
                    lst.Add(msg);
                }

                foreach (var msg in SelectedContact.Messages.Where(m => m.ToUserId == User.Id))
                {
                    msg.IsSelf = false;
                    lst.Add(msg);
                }

                lst.Sort((x, y) => DateTime.Compare(x.SendingTime, y.SendingTime));
                Messages.Clear();
                foreach (var msg in lst.OrderBy(m => m.SendingTime).ToList())
                {
                    Messages.Add(msg);
                }
            }
            else
                Messages.Clear();
        }

        public void UpdateUsers()
        {
            if (!string.IsNullOrWhiteSpace(FriendName))
            {
                
            }
            else
                Users.Clear();
        }

        public void SendMessage()
        {
            if (!string.IsNullOrWhiteSpace(Text) && SelectedContact != null)
            {
                var Message = new Message();
                Message.MessageType = "text";
                Message.FromUserId = User.Id;
                Message.FromUserImageSource = User.ImageSource;
                Message.FromUserUsername = User.Username;
                Message.ToUserId = SelectedContact.Id;
                Message.Data = Encoding.UTF8.GetBytes(Text);
                Message.SendingTime = DateTime.Now;
                Message.IsSelf = true;

                User.Messages.Add(Message);

                var str = $"message${JsonSerializer.Serialize(Message)}";

                Server.client.Client.Send(Encoding.UTF8.GetBytes(str));

                UpdateMessages();
            }

            Text = "";
        }
    }
}
