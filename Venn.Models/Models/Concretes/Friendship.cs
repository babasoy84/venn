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
    public class Friendship : Entity
    {
        [ForeignKey("User1")]
        public int User1Id { get; set; }

        [ForeignKey("User2")]
        public int User2Id { get; set; }

        [IgnoreDataMember]
        public User User1 { get; set; }

        [IgnoreDataMember]
        public User User2 { get; set; }
    }
}
