using Microsoft.AspNetCore.Components;
using SimpleInjector;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using Venn.Data;
using Venn.Data.Repos;
using Venn.Models.Models.Concretes;

namespace Venn.Server
{
    class Program
    {
        static int maxValue = ushort.MaxValue - 28;

        static Container container;

        static List<Client> clients;

        static TcpListener listener;

        static IRepository<User> userRepo;

        static IRepository<Room> roomRepo;

        static IRepository<Message> messageRepo;

        static ObservableCollection<User> users;

        static ObservableCollection<Room> rooms;

        static ObservableCollection<Message> messages;

        private static Dispatcher dispatcher = Dispatcher.CreateDefault();

        static void Main(string[] args)
        {
            container = new Container();
            container.RegisterSingleton<VennDbContext>();
            container.RegisterSingleton<IRepository<User>, Repository<User>>();
            container.RegisterSingleton<IRepository<Room>, Repository<Room>>();
            container.RegisterSingleton<IRepository<Message>, Repository<Message>>();
            userRepo = container.GetInstance<IRepository<User>>();
            roomRepo = container.GetInstance<IRepository<Room>>();
            messageRepo = container.GetInstance<IRepository<Message>>();
            users = new ObservableCollection<User>(userRepo.GetAll());
            rooms = new ObservableCollection<Room>(roomRepo.GetAll());
            messages = new ObservableCollection<Message>(messageRepo.GetAll());
            clients = new List<Client>();
            listener = new TcpListener(IPAddress.Parse("10.2.26.65"), 27001);

            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };

            listener.Start();
            Console.WriteLine($"[{DateTime.Now}]: Listener has started");

            while (true)
            {
                var newClient = new Client(listener.AcceptTcpClient());
                var ip = newClient.TcpClient.Client.RemoteEndPoint.ToString().Split(':')[0];
                Console.WriteLine($"[{ip}]: Client has connected");
                Task.Run(() =>
                {
                    var client = new Client(newClient.TcpClient);
                    clients.Add(client);
                    while (client.TcpClient.Connected)
                    {
                        string str;
                        try
                        {
                            var ns = client.TcpClient.GetStream();
                            var bytes = new byte[maxValue];
                            var length = ns.Read(bytes, 0, bytes.Length);
                            str = Encoding.Default.GetString(bytes, 0, length);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine($"[{ip}]: Client has disconnected");
                            clients.Remove(client);
                            foreach (var cl in clients)
                            {
                                if (cl.User != null)
                                {
                                    var contacts = new ObservableCollection<User>();
                                    foreach (var c in clients)
                                    {
                                        if (c.User != null && c.User.Id != cl.User.Id)
                                        {
                                            contacts.Add(c.User);
                                        }
                                    }

                                    var r = $"contacts${JsonSerializer.Serialize(contacts, options)}";

                                    if (Encoding.UTF8.GetBytes(r).Length > maxValue)
                                    {
                                        r = $"<{r}>";
                                        var data = Encoding.UTF8.GetBytes(r);
                                        var skipCount = 0;
                                        var bytesLen = data.Length;

                                        while (skipCount + maxValue <= bytesLen)
                                        {
                                            cl.TcpClient.Client.Send(data
                                                .Skip(skipCount)
                                                .Take(maxValue)
                                                .ToArray());
                                            skipCount += maxValue;
                                        }

                                        if (skipCount != bytesLen)
                                            cl.TcpClient.Client.Send(data
                                                .Skip(skipCount)
                                                .Take(bytesLen - skipCount)
                                                .ToArray());
                                    }
                                    else
                                    {
                                        cl.TcpClient.Client.Send(Encoding.UTF8.GetBytes(r));
                                    }
                                }
                            }
                            break;
                        }

                        var command = str.Split('$')[0];
                        if (command == "login")
                        {
                            var email = str.Split('$')[1];
                            var password = str.Split('$')[2];
                            var user = users.FirstOrDefault(u => u.Email == email);
                            if (user != null)
                            {
                                if (user.Password == password)
                                {
                                    var r = $"success${JsonSerializer.Serialize(user, options)}";

                                    if (Encoding.UTF8.GetBytes(r).Length > maxValue)
                                    {
                                        r = $"<{r}>";
                                        var data = Encoding.UTF8.GetBytes(r);
                                        var skipCount = 0;
                                        var bytesLen = data.Length;

                                        while (skipCount + maxValue <= bytesLen)
                                        {
                                            client.TcpClient.Client.Send(data
                                                .Skip(skipCount)
                                                .Take(maxValue)
                                                .ToArray());
                                            skipCount += maxValue;
                                        }

                                        if (skipCount != bytesLen)
                                            client.TcpClient.Client.Send(data
                                                .Skip(skipCount)
                                                .Take(bytesLen - skipCount)
                                                .ToArray());
                                    }
                                    else
                                    {
                                        client.TcpClient.Client.Send(Encoding.UTF8.GetBytes(r));
                                    }
                                    
                                    client.User = user;
                                    Console.WriteLine($"[{ip}]: Client has logined");
                                    foreach (var cl in clients)
                                    {
                                        if (cl.User != null)
                                        {
                                            var contacts = new ObservableCollection<User>();
                                            foreach (var c in clients)
                                            {
                                                if (c.User != null && c.User.Id != cl.User.Id)
                                                {
                                                    contacts.Add(c.User); 
                                                }
                                            }

                                            r = $"contacts${JsonSerializer.Serialize(contacts, options)}";

                                            if (Encoding.UTF8.GetBytes(r).Length > maxValue)
                                            {
                                                r = $"<{r}>";
                                                var data = Encoding.UTF8.GetBytes(r);
                                                var skipCount = 0;
                                                var bytesLen = data.Length;

                                                while (skipCount + maxValue <= bytesLen)
                                                {
                                                    cl.TcpClient.Client.Send(data
                                                        .Skip(skipCount)
                                                        .Take(maxValue)
                                                        .ToArray());
                                                    skipCount += maxValue;
                                                }

                                                if (skipCount != bytesLen)
                                                    cl.TcpClient.Client.Send(data
                                                        .Skip(skipCount)
                                                        .Take(bytesLen - skipCount)
                                                        .ToArray());
                                            }
                                            else
                                            {
                                                cl.TcpClient.Client.Send(Encoding.UTF8.GetBytes(r));
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    var r = "password$Sorry, your password was incorrect.";
                                    client.TcpClient.Client.Send(Encoding.UTF8.GetBytes(r));
                                    Console.WriteLine($"[{ip}]: Client login failed");
                                }
                            }
                            else
                            {
                                var r = "email$The email you entered isn’t connected to an account";
                                client.TcpClient.Client.Send(Encoding.UTF8.GetBytes(r));
                                Console.WriteLine($"[{ip}]: Client login failed");
                            }
                        }
                        else if (command == "create")
                        {
                            bool b = true;
                            var user = JsonSerializer.Deserialize<User>(str.Split('$')[1]);
                            foreach (var u in users)
                            {
                                if (u.Email == user.Email)
                                {
                                    b = false;
                                    break;
                                }
                            }
                            if (b)
                            {
                                lock (new object())
                                {
                                    userRepo.Add(user);
                                    userRepo.SaveChanges();
                                    users = new ObservableCollection<User>(userRepo.GetAll());
                                }
                                client.TcpClient.Client.Send(Encoding.UTF8.GetBytes("true"));
                                Console.WriteLine($"[{ip}]: Client has created user");
                            }
                            else
                            {
                                client.TcpClient.Client.Send(Encoding.UTF8.GetBytes("false"));
                                Console.WriteLine($"[{ip}]: Client create user failed");
                            }
                        }
                        else if (command == "message")
                        {
                            var message = JsonSerializer.Deserialize<Message>(str.Split('$')[1]);
                            client.User.Messages.Add(message);
                            messageRepo.Add(message);
                            messageRepo.SaveChanges();
                            userRepo.Update(client.User);
                            userRepo.SaveChanges();
                            users = new ObservableCollection<User>(userRepo.GetAll());
                            for (int i = 0; i < clients.Count(); i++)
                            {
                                clients[i].User = users.FirstOrDefault(u => u.Id == clients[i].User.Id);
                            }
                            foreach (var cl in clients)
                            {
                                if (cl.User != null)
                                {
                                    var contacts = new ObservableCollection<User>();
                                    foreach (var c in clients)
                                    {
                                        if (c.User != null && c.User.Id != cl.User.Id)
                                        {
                                            contacts.Add(c.User);
                                        }
                                    }

                                    var r = $"contacts${JsonSerializer.Serialize(contacts, options)}";

                                    if (Encoding.UTF8.GetBytes(r).Length > maxValue)
                                    {
                                        r = $"<{r}>";
                                        var data = Encoding.UTF8.GetBytes(r);
                                        var skipCount = 0;
                                        var bytesLen = data.Length;

                                        while (skipCount + maxValue <= bytesLen)
                                        {
                                            cl.TcpClient.Client.Send(data
                                                .Skip(skipCount)
                                                .Take(maxValue)
                                                .ToArray());
                                            skipCount += maxValue;
                                        }

                                        if (skipCount != bytesLen)
                                            cl.TcpClient.Client.Send(data
                                                .Skip(skipCount)
                                                .Take(bytesLen - skipCount)
                                                .ToArray());
                                    }
                                    else
                                    {
                                        cl.TcpClient.Client.Send(Encoding.UTF8.GetBytes(r));
                                    }
                                }
                            }
                        }
                    }
                });
            }
        }
    }
}