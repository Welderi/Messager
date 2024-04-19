using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Text;
using System.Threading.Tasks;
using DataBase;

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
        public void RemoveItem(ContactItem obj)
        {
            ContactsCollection.Remove(obj);
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
                ContactsCollection.Clear();

                var contactsForConcreteUser = dbContext.Contacts.Where(c => c.ConcreteUserID == id).ToList();
                var contactsForConcreteUser1 = dbContext.Contacts.Where(c => c.UserID == id).ToList();

                if (dbContext.GroupMemberships.FirstOrDefault(c => c.UserGMID == id) != null)
                {
                    var contactsForGroup = dbContext.GroupMemberships.FirstOrDefault(c => c.UserGMID == id);

                    var user = dbContext.Groups.FirstOrDefault(u => u.GroupID == contactsForGroup.GroupGMID);

                    var contactItem = new ContactItem { Name = user.Name, IsGroup = true };

                    AddItem(contactItem);
                }

                foreach (var contact in contactsForConcreteUser)
                {
                    var user = dbContext.Users.FirstOrDefault(u => u.UserID == contact.UserID);

                    var contactItem = new ContactItem { Name = user.Name,  IsGroup = false };

                    AddItem(contactItem);
                }
                foreach (var contact in contactsForConcreteUser1)
                {
                    var user = dbContext.Users.FirstOrDefault(u => u.UserID == contact.ConcreteUserID);

                    var contactItem = new ContactItem { Name = user.Name, IsGroup = false };

                    AddItem(contactItem);
                }


            }
        }
    }
    public class ContactItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        bool isGroup;

        public bool IsGroup
        {
            get { return isGroup; }
            set
            {
                isGroup = value;
                OnPropertyChanged(nameof(IsGroup));
            }
        }
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
