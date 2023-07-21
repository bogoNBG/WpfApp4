using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp4.Model
{
    internal class Option
    {
        public int Id { get; set; }        
        public string Name { get; set; }

        public Option(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
