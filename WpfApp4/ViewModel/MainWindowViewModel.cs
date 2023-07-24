using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using WpfApp4.Model;
using WpfApp4.MVVM;
using WpfApp4.Repository;
using System.Windows;

namespace WpfApp4.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {            
            Contacts = new ObservableCollection<ContactViewModel>();
            ShownContacts = Contacts;
            Options = new ObservableCollection<Option>();
            Placeholder = "Search";

            Repository = new MainRepository();
            Repository.CreateTables();
            Repository.LoadInfo(Contacts, Options);
            
        }
        MainRepository Repository { get; set; }
        public ObservableCollection<ContactViewModel> Contacts { get; set; }
        public ObservableCollection<Option> Options { get; set; }

        public RelayCommand AddContactCommand => new(execution => AddContact());
        public RelayCommand RemoveContactCommand => new(execution => RemoveContact(), canExecute => CanRemoveContact());
        public RelayCommand AddOptionCommand => new(execution => AddOption(), canExecute => CanAddOption());
        public RelayCommand RemoveOptionCommand => new(execution => RemoveOption(), canExecute => CanRemoveOption());
        public RelayCommand AddLinkCommand => new(execution => AddLink(), canExecute => CanAddLink());
        public RelayCommand SearchCommand => new(execution => SearchContacts());
        public RelayCommand SaveContactCommand => new(execution => SaveContact(), canExecute => CanSaveContact());

        private string name;
        public string Name
        {
            get { return name; }
            set 
            {
                name = value;
                OnPropertyChanged();
            }
        }

        private string number;
        public string Number
        {
            get { return number; }
            set 
            { 
                number = value;
                OnPropertyChanged();
            }
        }

        private string email;
        public string Email
        {
            get { return email; }
            set 
            { 
                email = value;
                OnPropertyChanged();
            }
        }

        private bool isAddContactSelected;

        public bool IsAddContactSelected
        {
            get { return isAddContactSelected; }
            set { isAddContactSelected = value; }
        }

        private string addContactText;

        public string AddContactText
        {
            get { return addContactText; }
            set
            {
                addContactText = value;
                OnPropertyChanged();
            }
        }



        private string optionName;
        public string OptionName
        {
            get { return optionName; }
            set 
            {
                optionName = value;
                OnPropertyChanged();
            }
        }

        private string linkName;
        public string LinkName
        {
            get { return linkName; }
            set 
            { 
                linkName = value;
                OnPropertyChanged();
            }
        }

        private string searchedContact;
        public string SearchedContact
        {
            get { return searchedContact; }
            set 
            { 
                searchedContact = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ContactViewModel> shownContacts;
        public ObservableCollection<ContactViewModel> ShownContacts
        {
            get { return shownContacts; }
            set
            {
                shownContacts = value;
                OnPropertyChanged();
            }
        }


        private ContactViewModel selectedContact;
        public ContactViewModel SelectedContact
		{
			get { return selectedContact; }
			set
            {
                selectedContact = value;
                if(SelectedContact != null) 
                {
                    Name = SelectedContact.Name;
                    Number = SelectedContact.Number;
                    Email = SelectedContact.Email;
                }
                IsAddContactSelected = false;
                AddContactText = "";
                OnPropertyChanged();
            }
		}

        private Option selectedOption;
        public Option SelectedOption
        {
            get { return selectedOption; }
            set 
            {
                selectedOption = value;
                OnPropertyChanged();
            }
        }

        private void AddContact()
        {
            SelectedContact = null;
            Name = "";
            Number = "";
            Email = "";
            IsAddContactSelected = true;
            AddContactText = "Adding new contact:";
        }
        //nigga balls

        private void RemoveContact()
        {
            Repository.DeleteRow(SelectedContact);
            Repository.RemoveLinksFromContact(SelectedContact);
            Contacts.Remove(SelectedContact);

            Name = "";
            Number = "";
            Email = "";
        }
        private bool CanRemoveContact()
        {
            if (SelectedContact != null){
                return true;
            }
            return false;
        }

        private void AddOption()
        {
            Option option = new Option(Repository.IdNum("Options"), this.OptionName);
            Options.Add(option);
            Repository.AddOption(option);
            OptionName = "";
        }
        private bool CanAddOption()
        {
            if(!string.IsNullOrWhiteSpace(OptionName))
            {
                return true;
            }
            return false;
        }

        private void RemoveOption()
        {
            var contactsCopy = new List<ContactViewModel>(Contacts);
            foreach (ContactViewModel contact in contactsCopy)
            {
                var newLinks = new List<LinkViewModel>(contact.Links);

                foreach (LinkViewModel link in contact.Links)
                {
                    if (link.OptionId == SelectedOption.Id)
                    {
                        newLinks.Remove(link);
                    }
                }

                contact.Links = newLinks;
            }
            Repository.RemoveLinksFromOptions(SelectedOption);
            Repository.RemoveOption(SelectedOption);
            Options.Remove(SelectedOption);
            
        }

            private bool CanRemoveOption()
        {
            if(SelectedOption != null)
            {
                return true;
            }
            return false;
        }

        private void SearchContacts()
        {
            if (string.IsNullOrEmpty(SearchedContact))
            {
                ShownContacts = Contacts;
            }
            else
            {
                ShownContacts = new ObservableCollection<ContactViewModel>
                    (Contacts.Where(c => c.Name.ToLower().Contains(SearchedContact.ToLower()))
                    
                ) ;
            }
        }

        //private void AddLink()
        //{
        //    Link link = new Link(Repository.IdNum("Links"), SelectedContact.Id, SelectedOption.Id, LinkName);
        //    SelectedContact.Links.Add(link);
        //    Repository.AddLink(link);
        //    LinkName = "";            
        //}

        private void AddLink()
        {
            Link link = new(Repository.IdNum("Links"), SelectedContact.Id, 0, " ");
            SelectedContact.Links.Add(new LinkViewModel(link));
            Repository.AddLink(new LinkViewModel(link));
        }
        private bool CanAddLink()
        {
            if (SelectedContact != null) return true;
            return false;
        }

        private void SaveContact()
        {
            if(IsAddContactSelected ==  false) 
            {
                SelectedContact.Name = Name;
                SelectedContact.Number = Number;
                SelectedContact.Email = Email;

                Repository.UpdateRow(SelectedContact);

                var linksCopy = new List<LinkViewModel>(SelectedContact.Links);
                foreach (LinkViewModel link in SelectedContact.Links)
                {
                    if (link.Name == "")
                    {
                        linksCopy.Remove(link);
                        Repository.RemoveLink(link);
                    }
                    Repository.UpdateLink(link);
                }
                SelectedContact.Links = linksCopy;

                SelectedContact = null;

                Name = "";
                Number = "";
                Email = "";
            }
            else
            {
                Contact contact = new Contact(Repository.IdNum("Contacts"), this.Name, this.Number, this.Email);
                Contacts.Add(new ContactViewModel(contact));
                Repository.AddRow(contact);
                Name = "";
                Number = "";
                Email = "";
                IsAddContactSelected = false;
                AddContactText = "";
            }

        }
        private bool CanSaveContact()
        {
            if (SelectedContact != null || (IsAddContactSelected == true && !string.IsNullOrWhiteSpace(Name) &&
                !string.IsNullOrWhiteSpace(Number) && !string.IsNullOrWhiteSpace(Email))) return true;
            else return false;
        }


        //Search...

        public RelayCommand ClearTextCommand => new(execution => ClearText());
        public RelayCommand IsSearchEmptyCommand => new(execution => IsEmpty());

        private string placeholder;

        public string Placeholder
        {
            get { return placeholder; }
            set
            {
                placeholder = value;
                OnPropertyChanged();
            }
        }

        public void ClearText()
        {
            SearchedContact = string.Empty;
        }

        public void IsEmpty()
        {
            if (string.IsNullOrWhiteSpace(SearchedContact))
            {
                Placeholder = "Search";
            }
            else
            {
                Placeholder = string.Empty;
            }
        }

        //...

    }
}
