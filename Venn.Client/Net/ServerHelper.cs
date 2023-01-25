using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public TcpClient client;

        public ServerHelper()
        {
            client = new TcpClient();
        }

        public void ConnectToServer()
        {
            if (!client.Connected)
            {
                client.Connect(IPAddress.Parse("192.168.56.1"), 27001);
            }
        }

        public string Login(string email, string password)
        {
            var r = "login$" + email + "$" + password;
            client.Client.Send(Encoding.UTF8.GetBytes(r));
            var ns = client.GetStream();
            var bytes = new byte[ushort.MaxValue - 28];
            var length = ns.Read(bytes, 0, bytes.Length);
            var str = Encoding.Default.GetString(bytes, 0, length);
            if (str[0] == '<')
            {
                while (true)
                {
                    ns = client.GetStream();
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
            return str;
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
