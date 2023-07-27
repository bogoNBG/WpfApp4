using WpfApp4.Model;
using WpfApp4.MVVM;

namespace WpfApp4.ViewModel
{
    class OptionViewModel : ViewModelBase
    {
        private Option option;
        public OptionViewModel(Option option)
        {
            this.option = option;
        }

        public int Id
        {
            get { return option.Id; }
            set
            {
                option.Id = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get { return option.Name; }
            set
            {
                option.Name = value;
                OnPropertyChanged();
            }
        }

        public override bool Equals(object obj)
        {
            if (obj as OptionViewModel == null)
            {
                return false;
            }

            return ((OptionViewModel)obj).Id == this.Id;
        }

        public override int GetHashCode()
        {
            return this.Id;
        }
    }
}
