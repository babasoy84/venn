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
using System.Windows;
using System.Windows.Threading;
using Venn.Client.Net;
using Venn.Client.Services;
using Venn.Models.Models.Concretes;

namespace Venn.Client.MVVM.ViewModels
{
    internal class ChatViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ManualResetEvent mainEvent;

        const int maxValue = ushort.MaxValue - 28;

        public User User { get; set; }


        public ServerHelper Server { get; set; }

        public INavigationService NavigationService { get; set; }


        private ObservableCollection<User> users;

        private ObservableCollection<Message> messages;

        private Friendship selectedContact;

        private int selectedContactIndex;

        private JsonSerializerOptions options;

        private string text;

        private string friendName;

        private bool friendsPopupIsOpen = false;

        private bool notificationsPopupIsOpen = false;


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

        public Friendship SelectedContact
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
                FindFriend();
            }
        }

        public bool FriendsPopupIsOpen
        {
            get { return friendsPopupIsOpen; }
            set
            {
                friendsPopupIsOpen = value;
                NotifyPropertyChanged("FriendsPopupIsOpen");
            }
        }

        public bool NotificationsPopupIsOpen
        {
            get { return notificationsPopupIsOpen; }
            set
            {
                notificationsPopupIsOpen = value;
                NotifyPropertyChanged("NotificationsPopupIsOpen");
            }
        }


        public Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

        public RelayCommand SendMessageCommand { get; set; }

        public RelayCommand<int> SendFriendshipCommand { get; set; }

        public RelayCommand<int> AcceptFriendshipCommand { get; set; }

        public RelayCommand LogoutCommand { get; set; }

        public RelayCommand OpenFriendsPopupCommand { get; set; }

        public RelayCommand OpenNotificationsPopupCommand { get; set; }

        public ChatViewModel()
        {
            options = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };
            User = App.Container.GetInstance<User>();
            Server = App.Container.GetInstance<ServerHelper>();
            NavigationService = App.Container.GetInstance<INavigationService>();
            Users = new ObservableCollection<User>();
            Messages = new ObservableCollection<Message>();
            SendMessageCommand = new RelayCommand(SendMessage);
            LogoutCommand = new RelayCommand(Logout);
            SendFriendshipCommand = new RelayCommand<int>(SendFriendship);
            AcceptFriendshipCommand = new RelayCommand<int>(AcceptFriendship);
            OpenFriendsPopupCommand = new RelayCommand(() => FriendsPopupIsOpen = true);
            OpenNotificationsPopupCommand = new RelayCommand(() => NotificationsPopupIsOpen = true);
            mainEvent = new ManualResetEvent(false);
            Task.Run(() =>
            {
                while (true)
                {
                    mainEvent.WaitOne();
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
                    if (command == "users")
                    {
                        lock (this)
                        {
                            dispatcher.Invoke(() => Users.Clear());
                            foreach (var user in JsonSerializer.Deserialize<ObservableCollection<User>>(str.Split('$')[1], options))
                            {
                                dispatcher.Invoke(() => Users.Add(user));
                            }
                        }
                    }
                    else if (command == "noti")
                    {
                        var noti = JsonSerializer.Deserialize<Notification>(str.Split("$")[1], options);
                        dispatcher.Invoke(() => User.Notifications.Add(noti));
                    }
                    else if (command == "addfs")
                    {
                        var fs = JsonSerializer.Deserialize<Friendship>(str.Split("$")[1], options);
                        dispatcher.Invoke(() => User.Contacts.Add(fs));
                    }
                    else if (command == "msg")
                    {
                        var msg = JsonSerializer.Deserialize<Message>(str.Split("$")[1], options);
                        dispatcher.Invoke(() => Messages.Add(msg));
                    }
                    else if (command == "logout")
                    {
                        continue;
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
            Messages.Clear();
            if (SelectedContact != null)
            {
                var lst = new List<Message>();
                foreach (var msg in User.Messages.Where(m => m.ToUserId == SelectedContact.User2.Id))
                {
                    msg.IsSelf = true;
                    msg.FromUser = User;
                    lst.Add(msg);
                }

                foreach (var msg in SelectedContact.User2.Messages.Where(m => m.ToUserId == User.Id))
                {
                    msg.IsSelf = false;
                    msg.FromUser = SelectedContact.User2;
                    lst.Add(msg);
                }

                lst.Sort((x, y) => DateTime.Compare(x.SendingTime, y.SendingTime));
                
                foreach (var msg in lst.OrderBy(m => m.SendingTime).ToList())
                {
                    Messages.Add(msg);
                }
            }
        }

        public void FindFriend()
        {
            if (!string.IsNullOrWhiteSpace(FriendName))
            {
                var str = $"users${FriendName}";

                Server.client.Client.Send(Encoding.UTF8.GetBytes(str));
            }
            else
                Users.Clear();
        }

        public void SendMessage()
        {
            if (!string.IsNullOrWhiteSpace(Text) && SelectedContact != null)
            {
                var msg = new Message();
                msg.MessageType = "text";
                msg.FromUserId = User.Id;
                msg.FromUser = User;
                msg.ToUserId = SelectedContact.User2Id;
                msg.ToUser = SelectedContact.User2;
                msg.Data = Encoding.UTF8.GetBytes(Text);
                msg.SendingTime = DateTime.Now;
                msg.IsSelf = true;

                User.Messages.Add(msg);
                Messages.Add(msg);

                var str = $"message${JsonSerializer.Serialize(msg, options)}";

                SendCommand(str);
            }

            Text = "";
        }

        public void Logout()
        {
            mainEvent.Reset();

            Server.client.Client.Send(Encoding.UTF8.GetBytes("logout"));

            NavigationService.NavigateTo<WelcomeViewModel>();
        }

        public void SendFriendship(int id)
        {
            var noti = new Notification();
            noti.FromUserId = User.Id;
            noti.ToUserId = id;
            noti.Text = $"[{User.Username}] sent you a friend request";
            noti.SendingTime = DateTime.Now;
            noti.FromUser = User;
            noti.ToUser = Users.FirstOrDefault(u => u.Id == id);

            var str = $"noti${JsonSerializer.Serialize(noti, options)}";

            SendCommand(str);
        }

        public void AcceptFriendship(int id)
        {
            var noti = User.Notifications.FirstOrDefault(n => n.Id == id);

            User.Notifications.Remove(noti);

            var fs = new Friendship()
            {
                User1Id = User.Id,
                User1 = User,
                User2Id = noti.FromUserId,
                User2 = noti.FromUser
            };

            User.Contacts.Add(fs);

            var str = $"addfs${JsonSerializer.Serialize(fs, options)}";

            SendCommand(str);
        }

        public void SendCommand(string str)
        {
            if (Encoding.UTF8.GetBytes(str).Length > maxValue)
            {
                str = $"<{str}>";
                var data = Encoding.UTF8.GetBytes(str);
                var skipCount = 0;
                var bytesLen = data.Length;

                while (skipCount + maxValue <= bytesLen)
                {
                    Server.client.Client.Send(data
                        .Skip(skipCount)
                        .Take(maxValue)
                        .ToArray());
                    skipCount += maxValue;
                }

                if (skipCount != bytesLen)
                    Server.client.Client.Send(data
                        .Skip(skipCount)
                        .Take(bytesLen - skipCount)
                        .ToArray());
            }
            else
            {
                Server.client.Client.Send(Encoding.UTF8.GetBytes(str));
            }
        }
    }
}
