using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp4.Model;
using WpfApp4.MVVM;

namespace WpfApp4.ViewModel
{
    class ContactViewModel : ViewModelBase
    {
        private Contact contact;

        public ContactViewModel(Contact contact)
        {
            this.contact = contact;
        }

        public string Name
        {
            get { return contact.Name; }
            set
            {
                if (contact.Name != value)
                {
                    contact.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string Number
        {
            get { return contact.Number; }
            set
            {
                if (contact.Number != value)
                {
                    contact.Number = value;
                    OnPropertyChanged(nameof(contact.Number));
                }
            }
        }

        public string Email
        {
            get { return contact.Email; }
            set
            {
                if (contact.Email != value)
                {
                    contact.Email = value;
                    OnPropertyChanged(nameof(contact.Email));
                }
            }
        }

        public List<Link> Links
        {
            get { return contact.Links; }
            set
            {
                if (contact.Links != value)
                {
                    contact.Links = value;
                    OnPropertyChanged(nameof(contact.Links));
                }
            }
        }
    }
}
