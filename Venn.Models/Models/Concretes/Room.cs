using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venn.Models.Models.Abstracts;

namespace Venn.Models.Models.Concretes
{
    public class Room : Entity
    {
        public string? ImageSource { get; set; }

        public string Name { get; set; }

        public List<User> Users { get; set; }

        public List<Message> Messages { get; set; }


        public Room()
        {
            
        }

        public Room(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
