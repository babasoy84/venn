using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Venn.Models.Models.Concretes;

namespace Venn.Server
{
    internal class Client
    {
        public TcpClient TcpClient { get; set; }

        public User User { get; set; }


        public Client(TcpClient tcpClient)
        {
            TcpClient = tcpClient;
        }
    }
}
