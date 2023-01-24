using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using Venn.Models.Models.Abstracts;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Venn.Models.Models.Concretes
{
    public class Message : Entity
    {
        [ForeignKey("FromUser")]
        public int FromUserId { get; set; }

        [ForeignKey("ToUser")]
        public int ToUserId { get; set; }

        public byte[] Data { get; set; }

        public string MessageType { get; set; }

        public DateTime SendingTime { get; set; }

        public bool IsSelf { get; set; } = true;

        [IgnoreDataMember]
        public virtual User FromUser { get; set; }

        [IgnoreDataMember]
        public virtual User ToUser { get; set; }

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
