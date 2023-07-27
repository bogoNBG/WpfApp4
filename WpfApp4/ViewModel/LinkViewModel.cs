using System;
using WpfApp4.Model;
using WpfApp4.MVVM;
using WpfApp4.Repository;

namespace WpfApp4.ViewModel
{
    class LinkViewModel : ViewModelBase
    {
        private readonly MainRepository repository;
        private readonly Link link;
        private OptionViewModel option;

        public LinkViewModel(Link link, MainRepository repository)
        {
            this.link = link;
            this.repository = repository;

            var option = this.repository.GetOptionByIdFromDB(this.link.OptionId);

            if (option != null)
            {
                this.Option = new OptionViewModel(option);
            }

            //this.Option = new OptionViewModel(option);
        }

        public int Id
        {
            get { return link.Id; }
            set
            {
                link.Id = value;
                OnPropertyChanged();
            }
        }

        public int ContactId
        {
            get { return link.ContactId; }
            set
            {
                if (link.ContactId != value)
                {
                    link.ContactId = value;
                    OnPropertyChanged();
                }
            }
        }

        public OptionViewModel Option
        {
            get { return this.option; }
            set
            {
                this.option = value;
                this.OnPropertyChanged();
            }
        }

        public string Value
        {
            get { return link.Value; }
            set
            {
                link.Value = value;
                OnPropertyChanged();                
            }
        }

        public bool IsAssigned
        {
            get { return link.IsAssigned; }
            set 
            {
                link.IsAssigned = value; 
                OnPropertyChanged(); 
            }
        }


    }
}
