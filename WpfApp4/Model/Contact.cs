﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp4.MVVM;
using WpfApp4.ViewModel;

namespace WpfApp4.Model
{
    internal class Contact
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

        public Contact(string name, string number, string email)
        {
            this.Name = name;
            this.Number = number;
            this.Email = email;
            Links = new List<Link>();
        }

    }
}
