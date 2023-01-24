using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Venn.Models.Models.Abstracts;

namespace Venn.Models.Models.Concretes
{
    public class Notification : Entity
    {
        [ForeignKey("FromUser")]
        public int FromUserId { get; set; }

        [ForeignKey("ToUser")]
        public int ToUserId { get; set; }

        public string Text { get; set; }

        public DateTime SendingTime { get; set; }

        [IgnoreDataMember]
        public virtual User FromUser { get; set; }

        [IgnoreDataMember]
        public virtual User ToUser { get; set; }
    }
}
