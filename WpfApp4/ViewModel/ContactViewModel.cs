using System.Collections.ObjectModel;
using WpfApp4.Model;
using WpfApp4.MVVM;
using WpfApp4.Repository;

namespace WpfApp4.ViewModel
{
    class ContactViewModel : ViewModelBase
    {
        private readonly MainRepository repository;

        private Contact contact;
        private ObservableCollection<LinkViewModel> links;
        public ContactViewModel(Contact contact, MainRepository repository)
        {
            this.repository = repository;
            this.contact = contact;
            
            

            if (this.contact?.Links != null)
            {
                // this.links = new ObservableCollection<LinkViewModel>(this.contact.Links.Select(l => new LinkViewModel(l)));
                this.links = new ObservableCollection<LinkViewModel>();

                foreach (var link in this.contact.Links)
                {
                    this.links.Add(new LinkViewModel(link, this.repository));
                }
            }
        }

        public int Id
        {
            get { return contact.Id; }
            set { contact.Id = value; }
        }


        public string Name
        {
            get { return contact.Name; }
            set
            {
                if (contact.Name != value)
                {
                    contact.Name = value;
                    OnPropertyChanged();
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
                    OnPropertyChanged();
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
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<LinkViewModel> Links
        {
            get { return this.links; }
            set
            {
                this.links = value;
                OnPropertyChanged();
            }
        }

        public void RefreshLinks()
        {
            this.Links.Clear();
            this.repository.GetContactsLinksFromDB(contact, Links);
        }


    }
}
