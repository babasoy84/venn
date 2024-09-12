using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Principal;
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
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Web;
using System.Net;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using System.Xml.Linq;
using Azure;
using System.Collections.Specialized;
using System.Windows.Controls;
using Google.Cloud.Translation.V2;
using Haley.Abstractions;
using Haley.Services;

namespace Venn.Client.MVVM.ViewModels
{
    internal class ChatViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ManualResetEvent mainEvent;

        const int maxValue = ushort.MaxValue - 28;

        public User User { get; set; }

        public Cloudinary Cloudinary { get; set; }

        public ServerHelper Server { get; set; }

        public INavigationService NavigationService { get; set; }

        public SnackbarMessageQueue SnackbarMessageQueue { set; get; } = new(TimeSpan.FromSeconds(1));

        private TranslationClient client = TranslationClient.CreateFromApiKey("AIzaSyApuoMkkiWC_ZbAzIHg_eF64fFeJ42OJDw");

        private IDialogService _ds;


        private ObservableCollection<User> users;

        private ObservableCollection<Message> messages;

        private ObservableCollection<Venn.Models.Models.Concretes.Language> languages;

        private Venn.Models.Models.Concretes.Language selectedLanguage;

        private Friendship selectedContact;

        private int selectedContactIndex;

        private JsonSerializerOptions options;

        private string text;

        private string friendName;

        private bool friendsPopupIsOpen = false;

        private bool notificationsPopupIsOpen = false;

        private bool openFilePopupIsOpen = false;

        private bool displayFilePopupIsOpen = false;

        private bool loadingPopupIsOpen = false;

        private BitmapSource fileIcon;

        private string videoSource;

        private string fileName;

        private string fileSize;

        private Visibility videoVisibility;

        private Visibility imageVisibility;

        
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

        public ObservableCollection<Venn.Models.Models.Concretes.Language> Languages
        {
            get { return languages; }
            set
            {
                languages = value;
                NotifyPropertyChanged("Languages");
            }
        }

        public Venn.Models.Models.Concretes.Language SelectedLanguage
        {
            get { return selectedLanguage; }
            set
            {
                selectedLanguage = value;
                NotifyPropertyChanged("SelectedLanguage");
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

        public bool OpenFilePopupIsOpen
        {
            get { return openFilePopupIsOpen; }
            set
            {
                openFilePopupIsOpen = value;
                NotifyPropertyChanged("OpenFilePopupIsOpen");
            }
        }

        public bool DisplayFilePopupIsOpen
        {
            get { return displayFilePopupIsOpen; }
            set
            {
                displayFilePopupIsOpen = value;
                NotifyPropertyChanged("DisplayFilePopupIsOpen");
            }
        }

        public bool LoadingPopupIsOpen
        {
            get { return loadingPopupIsOpen; }
            set
            {
                loadingPopupIsOpen = value;
                NotifyPropertyChanged("LoadingPopupIsOpen");
            }
        }

        public BitmapSource FileIcon
        {
            get { return fileIcon; }
            set
            {
                fileIcon = value;
                NotifyPropertyChanged("FileIcon");
            }
        }

        public string VideoSource
        {
            get { return videoSource; }
            set
            {
                videoSource = value;
                NotifyPropertyChanged("VideoSource");
            }
        }

        public string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                NotifyPropertyChanged("FileName");
            }
        }

        public string FileSize
        {
            get { return fileSize; }
            set
            {
                fileSize = value;
                NotifyPropertyChanged("FileSize");
            }
        }

        public string ImageSource
        {
            get { return User.ImageSource; }
            set
            {
                User.ImageSource = value;
                NotifyPropertyChanged("ImageSource");
            }
        }

        public Visibility VideoVisibility
        {
            get { return videoVisibility; }
            set
            {
                videoVisibility = value;
                NotifyPropertyChanged("VideoVisibility");
            }
        }

        public Visibility ImageVisibility
        {
            get { return imageVisibility; }
            set
            {
                imageVisibility = value;
                NotifyPropertyChanged("ImageVisibility");
            }
        }

        public string Username
        {
            get { return User.Username; }
            set
            {
                User.Username = value;
                NotifyPropertyChanged("Username");
            }
        }

        public string FileType { get; set; }

        public string FilePath { get; set; }


        public Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

        public RelayCommand SendMessageCommand { get; set; }

        public RelayCommand<int> SendFriendshipCommand { get; set; }

        public RelayCommand<int> AcceptFriendshipCommand { get; set; }

        public RelayCommand LogoutCommand { get; set; }

        public RelayCommand OpenFriendsPopupCommand { get; set; }

        public RelayCommand OpenNotificationsPopupCommand { get; set; }

        public RelayCommand UploadProfilPhotoCommand { get; set; }

        public RelayCommand OpenFileCommand { get; set; }

        public RelayCommand SendFileCommand { get; set; }

        public RelayCommand<string> DisplayImageCommand { get; set; }

        public RelayCommand<string> DisplayVideoCommand { get; set; }

        public ChatViewModel()
        {
            options = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };
            _ds = new DialogService();
            Account account = new Account("dv9hubcxy", "956258466281318", "wG5We44Sc-9SThiN68YKgZ8HPbY");
            Cloudinary = new Cloudinary(account);
            User = App.Container.GetInstance<User>();
            Server = App.Container.GetInstance<ServerHelper>();
            NavigationService = App.Container.GetInstance<INavigationService>();
            Languages = new() {
                new Models.Models.Concretes.Language(null, "Original"),
                new Models.Models.Concretes.Language("az", "Azerbaijani"),
                new Models.Models.Concretes.Language("en", "English"),
                new Models.Models.Concretes.Language("tr", "Turkish"),
                new Models.Models.Concretes.Language("ru", "Russian"),
                new Models.Models.Concretes.Language("es", "Spanish"),
                new Models.Models.Concretes.Language("fr", "French")
            };
            Users = new ObservableCollection<User>();
            Messages = new ObservableCollection<Message>();
            SendMessageCommand = new RelayCommand(() => Task.Run(() => SendMessage()));
            LogoutCommand = new RelayCommand(Logout);
            SendFriendshipCommand = new RelayCommand<int>(SendFriendship);
            AcceptFriendshipCommand = new RelayCommand<int>(AcceptFriendship);
            OpenFileCommand = new RelayCommand(OpenFile);
            SendFileCommand = new RelayCommand(() => Task.Run(() => SendFile()));
            DisplayImageCommand = new RelayCommand<string>(DisplayImage);
            DisplayVideoCommand = new RelayCommand<string>(DisplayVideo);
            OpenNotificationsPopupCommand = new RelayCommand(() =>
            {
                if (User.Notifications.Count() == 0)
                    SnackbarEnqueue("You have no any notifications");
                else
                    NotificationsPopupIsOpen = true;
            });
            OpenFriendsPopupCommand = new RelayCommand(() => FriendsPopupIsOpen = true);
            UploadProfilPhotoCommand = new RelayCommand(() =>
            {
                UploadProfilPhoto();
                UpdatedUser();
            });
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
                            foreach (var user in System.Text.Json.JsonSerializer.Deserialize<ObservableCollection<User>>(str.Split('$')[1], options))
                            {
                                dispatcher.Invoke(() => Users.Add(user));
                            }
                        }
                    }
                    else if (command == "noti")
                    {
                        var noti = System.Text.Json.JsonSerializer.Deserialize<Notification>(str.Split("$")[1], options);
                        dispatcher.Invoke(() => User.Notifications.Add(noti));
                        SnackbarEnqueue(noti.Text);
                    }
                    else if (command == "addfs")
                    {
                        var fs = System.Text.Json.JsonSerializer.Deserialize<Friendship>(str.Split("$")[1], options);
                        dispatcher.Invoke(() => User.Contacts.Add(fs));
                    }
                    else if (command == "msg")
                    {
                        var msg = System.Text.Json.JsonSerializer.Deserialize<Message>(str.Split("$")[1], options);
                        dispatcher.Invoke(() => Messages.Add(msg));
                        dispatcher.Invoke(() => SelectedContact.User2.Messages.Add(msg));
                        bool b = true;
                        dispatcher.Invoke(() => b = Application.Current.MainWindow.IsActive);
                        if (!b)
                        {
                            dispatcher.Invoke(() => _ds.SendToast(msg.FromUser.Username, msg.Data, Haley.Enums.NotificationIcon.Info, false, true, 3));
                        }
                    }
                    else if (command == "update")
                    {
                        var usr = System.Text.Json.JsonSerializer.Deserialize<User>(str.Split("$")[1], options);
                        for (int i = 0; i < User.Contacts.Count(); i++)
                        {
                            if (User.Contacts[i].User2.Id == usr.Id)
                            {
                                User.Contacts[i].User2.ImageSource = usr.ImageSource;
                                User.Contacts[i].User2.Username = usr.Username;
                            }
                        }
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

        public async void SendMessage()
        {
            var txt = Text;
            Text = "";
            if (!string.IsNullOrWhiteSpace(txt) && SelectedContact != null)
            {
                var msg = new Message();
                msg.MessageType = "text";
                msg.FromUserId = User.Id;
                msg.FromUser = User;
                msg.ToUserId = SelectedContact.User2Id;
                msg.ToUser = SelectedContact.User2;
                msg.Data = txt;
                msg.SendingTime = DateTime.Now;
                msg.IsSelf = true;

                if (SelectedLanguage.Key != null)
                    msg.Data = await Translate(txt);

                dispatcher.Invoke(() => User.Messages.Add(msg));
                dispatcher.Invoke(() => Messages.Add(msg));

                var str = $"message${System.Text.Json.JsonSerializer.Serialize(msg, options)}";

                SendCommand(str);
            }
        }

        public void Logout()
        {
            Username = "";
            ImageSource = "";

            mainEvent.Reset();

            Messages.Clear();

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

            var str = $"noti${System.Text.Json.JsonSerializer.Serialize(noti, options)}";

            SendCommand(str);

            FriendsPopupIsOpen = false;

            SnackbarEnqueue($"Your friend request has been sent to [{noti.ToUser.Username}]");
        }

        public void AcceptFriendship(int id)
        {
            NotificationsPopupIsOpen = false;

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

            var str = $"addfs${System.Text.Json.JsonSerializer.Serialize(fs, options)}";

            SendCommand(str);
        }

        public void UploadProfilPhoto()
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            op.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (op.ShowDialog() == true)
            {
                using (var stream = File.OpenRead(op.FileName))
                {
                    ImageUploadParams uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(op.FileName, stream)
                    };

                    ImageSource = Cloudinary.Upload(uploadParams).Uri.AbsoluteUri;
                }
            }
        }

        public void OpenFile() 
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a file";
            op.Filter = "All Files (*.*)|*.*";
            op.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (op.ShowDialog() == true)
            {
                bool b = true;
                FilePath = op.FileName;

                FileInfo fileInfo = new FileInfo(op.FileName);

                long fsize = fileInfo.Length;

                using System.Drawing.Icon sysicon = System.Drawing.Icon.ExtractAssociatedIcon(op.FileName);

                string fileExtension = Path.GetExtension(op.FileName).ToLowerInvariant();
                string[] imageFileExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".jfif" };
                string[] videoExtensions = { ".mp4", ".mov", ".avi", ".wmv", ".mkv" };

                if (imageFileExtensions.Contains(fileExtension))
                {
                    if ((double)fsize / 1048576 > 10)
                    {
                        b = false;
                    }
                    else
                    {
                        FileType = "image";
                        ImageVisibility = Visibility.Visible;
                        VideoVisibility = Visibility.Collapsed;
                        FileIcon = new BitmapImage(new Uri(op.FileName, UriKind.Absolute));
                    }
                }
                else if (videoExtensions.Contains(fileExtension))
                {
                    if ((double)fsize / 1048576 > 100)
                    {
                        b = false;
                    }
                    else
                    {
                        FileType = "video";
                        ImageVisibility = Visibility.Collapsed;
                        VideoVisibility = Visibility.Visible;
                        VideoSource = op.FileName;
                    }
                }
                else
                {
                    if ((double)fsize / 1048576 > 10)
                    {
                        b = false;
                    }
                    else
                    {
                        FileType = "file";
                        ImageVisibility = Visibility.Visible;
                        VideoVisibility = Visibility.Collapsed;
                        FileIcon = Imaging.CreateBitmapSourceFromHIcon(
                              sysicon.Handle,
                              Int32Rect.Empty,
                              BitmapSizeOptions.FromEmptyOptions());
                    }
                }


                if (b)
                {
                    FileName = Path.GetFileName(op.FileName);

                    if (fsize >= 1073741824)
                    {
                        FileSize = string.Format("{0:0.##} GB", (double)fsize / 1073741824);
                    }
                    else if (fsize >= 1048576)
                    {
                        FileSize = string.Format("{0:0.##} MB", (double)fsize / 1048576);
                    }
                    else if (fsize >= 1024)
                    {
                        FileSize = string.Format("{0:0.##} KB", (double)fsize / 1024);
                    }
                    else
                    {
                        FileSize = string.Format("{0} bytes", fileSize);
                    }


                    OpenFilePopupIsOpen = true;
                }
            }
        }

        public void SendFile()
        {
            OpenFilePopupIsOpen = false;

            if (SelectedContact != null)
            {
                LoadingPopupIsOpen = true;
                var msg = new Message();
                msg.MessageType = FileType;
                msg.FromUserId = User.Id;
                msg.FromUser = User;
                msg.ToUserId = SelectedContact.User2Id;
                msg.ToUser = SelectedContact.User2;
                msg.SendingTime = DateTime.Now;
                msg.IsSelf = true;

                using var stream = File.OpenRead(FilePath);

                if (FileType == "image")
                {
                    ImageUploadParams uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(FilePath, stream)
                    };
                    msg.Data = Cloudinary.Upload(uploadParams).Uri.AbsoluteUri;
                }
                else if (FileType == "video")
                {
                    VideoUploadParams uploadParams = new VideoUploadParams()
                    {
                        File = new FileDescription(FilePath, stream)
                    };
                    msg.Data = Cloudinary.Upload(uploadParams).Uri.AbsoluteUri;
                }
                else
                {
                    RawUploadParams uploadParams = new RawUploadParams()
                    {
                        File = new FileDescription(FilePath, stream)
                    };
                    msg.Data = Cloudinary.Upload(uploadParams).Uri.AbsoluteUri;
                }

                

                dispatcher.Invoke(() => User.Messages.Add(msg));
                dispatcher.Invoke(() => Messages.Add(msg));

                var str = $"message${System.Text.Json.JsonSerializer.Serialize(msg, options)}";

                SendCommand(str);
                LoadingPopupIsOpen = false;
            }
        }

        public void DisplayImage(string imageSource)
        {
            ImageVisibility = Visibility.Visible;
            VideoVisibility = Visibility.Collapsed;
            VideoSource = null;
            FileIcon = new BitmapImage(new Uri(imageSource, UriKind.Absolute));

            DisplayFilePopupIsOpen = true;
        }

        public void DisplayVideo(string videoSource)
        {
            ImageVisibility = Visibility.Collapsed;
            VideoVisibility = Visibility.Visible;
            VideoSource = videoSource;

            DisplayFilePopupIsOpen = true;
        }

        public void UpdatedUser()
        {
            var str = $"update${System.Text.Json.JsonSerializer.Serialize(User, options)}";

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

        public async Task<string> Translate(string txt)
        {
            var reponse = client.TranslateText(txt, SelectedLanguage.Key);
            return reponse.TranslatedText;
        }

        public void SnackbarEnqueue(string msg, string btnContent = "", Action btnAction = null, double duration = 1)
        {
            SnackbarMessageQueue.Enqueue(msg,
            btnContent,
            _ => btnAction?.Invoke(), actionArgument: null,
            promote: false, neverConsiderToBeDuplicate: false,
            durationOverride: TimeSpan.FromSeconds(duration));
        }
    }
}