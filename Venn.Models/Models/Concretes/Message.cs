using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using Venn.Models.Models.Abstracts;

namespace Venn.Models.Models.Concretes
{
    public class Message : Entity
    {
        public int FromUserId { get; set; }

        public string FromUserImageSource { get; set; }

        public string FromUserUsername { get; set; }

        public int? ToUserId { get; set; }

        public int? ToRoomId { get; set; }

        public byte[] Data { get; set; }

        public string MessageType { get; set; }

        public DateTime SendingTime { get; set; }

        public bool IsSelf { get; set; } = true;

        public User FromUser { get; set; }

        public User? ToUser { get; set; }

        public Room? ToRoom { get; set; }

        public Message()
        {
            
        }

        public override string ToString()
        {
            if (MessageType == "text")
            {
                return Encoding.UTF8.GetString(Data);
            }
            return "image";
        }
    }
}
