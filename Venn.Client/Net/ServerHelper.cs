using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Venn.Models.Models.Concretes;

namespace Venn.Client.Net
{
    public class ServerHelper
    {
        TcpClient client;

        public ServerHelper()
        {
            client = new TcpClient();
        }

        public void ConnectToServer()
        {
            if (!client.Connected)
            {
                client.Connect(IPAddress.Parse("192.168.100.56"), 53685);
            }
        }

        public string Login(string email, string password)
        {
            var str = "login$" + email + "$" + password;
            client.Client.Send(Encoding.UTF8.GetBytes(str));
            var ns = client.GetStream();
            var bytes = new byte[4096];
            var length = ns.Read(bytes, 0, bytes.Length);
            return Encoding.Default.GetString(bytes, 0, length);
        }

        public bool CreateTeam(User user)
        {
            if (client.Connected)
            {
                var str = $"create${JsonSerializer.Serialize(user)}";
                client.Client.Send(Encoding.Default.GetBytes(str));
                var ns = client.GetStream();
                var bytes = new byte[4096];
                var length = ns.Read(bytes, 0, bytes.Length);
                var r = Encoding.Default.GetString(bytes, 0, length);
                if (r == "true")
                {
                    return true;
                }
            }
            return false;
        }
    }
}
