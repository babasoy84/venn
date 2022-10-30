using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venn.Models.Models.Abstracts;

namespace Venn.Models.Models.Concretes
{
    public class User : Entity
    {
        public string? ImageSource { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public DateTime DateOfBirth { get; set; }

        public ObservableCollection<Message> Messages { get; set; }

        public ObservableCollection<Room> Rooms { get; set; }

        public User()
        {
            Messages = new ObservableCollection<Message>();
        }

        public override string ToString()
        {
            return Username;
        }
    }
}
