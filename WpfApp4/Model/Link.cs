using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp4.Model
{
    internal class Link
    {
        public int Id { get; set; }
        public int OptionId { get; set; }
        public string Name { get; set; }

        public Link( int optionId, string name)
        {
            this.Id = IdGenerator.GetNextId<Link>();
            this.OptionId = optionId;
            this.Name = name;
        }

    }
}
