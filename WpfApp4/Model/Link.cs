using WpfApp4.MVVM;

namespace WpfApp4.Model
{
    internal class Link : ViewModelBase
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public int OptionId { get; set; }
        public string Value { get; set; }
        public bool IsAssigned { get; set; }

        public Link(int id,int contactId, int optionId, string value)
        {
            this.Id = id;
            this.ContactId = contactId;
            this.OptionId = optionId;
            this.Value = value;
            this.IsAssigned = false;
        }

        public Link(int contactId, string value)
        {
            this.ContactId = contactId;
            this.Value = value;
        }
    }
}
