﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp4.MVVM;


namespace WpfApp4.Model
{
    internal class Contact : ViewModelBase
    {
        public int Id { get; set; }
        public string Name { get; set; }       
        public string Number { get; set; }
        public string Email { get; set; }
        public List<Link> Links { get; set; }

        public Contact(int id, string name, string number, string email)
        {
            this.Id = id;
            this.Name = name;
            this.Number = number;
            this.Email = email;
            Links = new List<Link>();
        }

    }
}
