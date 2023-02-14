using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venn.Models.Models.Concretes
{
    public class Language
    {
        public string Key { get; set; }
        public string Name { get; set; }

        public Language()
        {

        }

        public Language(string key, string name)
        {
            Key = key;
            Name = name;
        }

        public override string ToString() => Name;
    }
}
