using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp4.Model;
using WpfApp4.MVVM;

namespace WpfApp4.ViewModel
{
    class LinkViewModel : ViewModelBase
    {
        private Link link;

        public LinkViewModel(Link link)
        {
            this.link = link;
        }

        public int Id
        {
            get { return link.Id; }
            set { link.Id = value; }
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

        public int OptionId
        {
            get { return link.OptionId; }
            set
            {
                if (link.OptionId != value)
                {
                    link.OptionId = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Name
        {
            get { return link.Name; }
            set
            {
                if (link.Name != value)
                {
                    link.Name = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
