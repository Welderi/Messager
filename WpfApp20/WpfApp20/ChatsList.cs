using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp20
{
    // Left panel with contacts
    public class ChatsList : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<ContactItem> ContactsCollection { get; set; } = new ObservableCollection<ContactItem>();

        public void AddItem(ContactItem obj)
        {
            ContactsCollection.Add(obj);
            OnPropertyChanged(nameof(ContactsCollection));   
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void AddContactsFromDatabase(int id)
        {
            using (var dbContext = new DataBaseDbContext())
            {
                var contactsForConcreteUser = dbContext.Contacts.Where(c => c.ConcreteUserID == id).ToList();
                var contactsForConcreteUser1 = dbContext.Contacts.Where(c => c.UserID == id).ToList();

                ContactsCollection.Clear();

                foreach (var contact in contactsForConcreteUser)
                {
                    var user = dbContext.Users.FirstOrDefault(u => u.UserID == contact.UserID);

                    var contactItem = new ContactItem { Name = user.Name };

                    AddItem(contactItem);
                }
                foreach (var contact in contactsForConcreteUser1)
                {
                    var user = dbContext.Users.FirstOrDefault(u => u.UserID == contact.ConcreteUserID);

                    var contactItem = new ContactItem { Name = user.Name };

                    AddItem(contactItem);
                }
            }
        }
    }
    public class ContactItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
