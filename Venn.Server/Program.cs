using SimpleInjector;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Venn.Data;
using Venn.Data.Repos;
using Venn.Models.Models.Concretes;

namespace Venn.Server
{
    class Program
    {
        static Container container;

        static List<Client> clients;

        static TcpListener listener;

        static IRepository<User> userRepo;

        static IRepository<Room> roomRepo;

        static ObservableCollection<User> users;

        static ObservableCollection<Room> rooms;

        static void Main(string[] args)
        {
            container = new Container();
            container.RegisterSingleton<VennDbContext>();
            container.RegisterSingleton<IRepository<User>, Repository<User>>();
            container.RegisterSingleton<IRepository<Room>, Repository<Room>>();
            userRepo = container.GetInstance<IRepository<User>>();
            roomRepo = container.GetInstance<IRepository<Room>>();
            users = new ObservableCollection<User>(userRepo.GetAll());
            rooms = new ObservableCollection<Room>(roomRepo.GetAll());
            clients = new List<Client>();
            listener = new TcpListener(IPAddress.Parse("192.168.100.56"), 53685);
            listener.Start();
            Console.WriteLine($"[{DateTime.Now}]: Listener has started");

            while (true)
            {
                var client = new Client(listener.AcceptTcpClient());
                Console.WriteLine($"[{DateTime.Now}]: Client has connected");
                clients.Add(client);
                Task.Run(() =>
                {
                    while (client.TcpClient.Connected)
                    {
                        var ns = client.TcpClient.GetStream();
                        var bytes = new byte[4096];
                        var length = ns.Read(bytes, 0, bytes.Length);
                        var str = Encoding.Default.GetString(bytes, 0, length);
                        var command  = str.Split('$')[0];
                        if (command == "login")
                        {
                            var email = str.Split('$')[1];
                            var password = str.Split('$')[2];
                            var user = users.FirstOrDefault(u => u.Email == email);
                            if (user != null)
                            {
                                if (user.Password == password)
                                {
                                    var r = $"success${JsonSerializer.Serialize(user)}";
                                    client.TcpClient.Client.Send(Encoding.UTF8.GetBytes(r));
                                    client.User = user;
                                    Console.WriteLine($"[{DateTime.Now}]: Client has logined");
                                }
                                else
                                {
                                    var r = "password$Sorry, your password was incorrect.";
                                    client.TcpClient.Client.Send(Encoding.UTF8.GetBytes(r));
                                    Console.WriteLine($"[{DateTime.Now}]: Client login failed");
                                }
                            }
                            else
                            {
                                var r = "email$The email you entered isn’t connected to an account";
                                client.TcpClient.Client.Send(Encoding.UTF8.GetBytes(r));
                                Console.WriteLine($"[{DateTime.Now}]: Client login failed");
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
                                Console.WriteLine($"[{DateTime.Now}]: Client has created user");
                            }  
                            else
                            {
                                client.TcpClient.Client.Send(Encoding.UTF8.GetBytes("false"));
                                Console.WriteLine($"[{DateTime.Now}]: Client create user failed");
                            }
                        }
                    }
                });
            }
        }
    }
}