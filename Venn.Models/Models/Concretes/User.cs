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
            ImageSource = "https://t4.ftcdn.net/jpg/00/64/67/63/360_F_64676383_LdbmhiNM6Ypzb3FM4PPuFP9rHe7ri8Ju.jpg";
            Messages = new ObservableCollection<Message>();
        }

        public override string ToString()
        {
            string str = "#";
            if (Id < 10) str = $"#000{Id}";
            else if (Id < 100) str = $"#00{Id}";
            else if (Id < 1000) str = $"#0{Id}";
            else str = $"#{Id}";
            return str;
        }
    }
}
