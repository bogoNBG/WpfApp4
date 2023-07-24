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
        public int ContactId { get; set; }

        public int OptionId { get; set; }
        public string Name { get; set; }

        public Link(int id,int contactId, int optionId, string name)
        {
            this.Id = id;
            this.ContactId = contactId;
            this.OptionId = optionId;
            this.Name = name;
        }
    }
}
