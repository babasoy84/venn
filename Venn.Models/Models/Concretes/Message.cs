using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using Venn.Models.Models.Abstracts;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Drawing;
using System.IO;

namespace Venn.Models.Models.Concretes
{
    public class Message : Entity
    {
        public int FromUserId { get; set; }

        public int ToUserId { get; set; }

        public string Data { get; set; }

        public string MessageType { get; set; }

        public DateTime SendingTime { get; set; }

        public bool IsSelf { get; set; } = true;

        public virtual User FromUser { get; set; }

        public virtual User ToUser { get; set; }

    }
}
