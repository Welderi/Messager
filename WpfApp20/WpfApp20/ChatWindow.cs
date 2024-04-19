using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using DataBase;

namespace WpfApp20
{
    // Right panel with chat
    public class ChatWindow : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<ChatItem> MessagesCollection { get; set; } = new ObservableCollection<ChatItem>();

        public void AddItem(ChatItem obj)
        {
            MessagesCollection.Add(obj);
            OnPropertyChanged(nameof(MessagesCollection));
        }
        public void DisplayConversation(int Userid, int concUserId)
        {
            MessagesCollection.Clear();

            using (var dbContext = new DataBaseDbContext())
            {
                var messages = dbContext.Messages.Where(m => m.SenderID == concUserId && m.RecipientID == Userid).ToList();
                var messages2 = dbContext.Messages.Where(m => m.SenderID == Userid && m.RecipientID == concUserId).ToList();

                foreach (var message in messages)
                {
                    var chatItem = new ChatItem { Message = message.Content };
                    AddItem(chatItem);
                }
                foreach (var message in messages2)
                {
                    var chatItem = new ChatItem { Message = message.Content };
                    AddItem(chatItem);
                }
            }
        }
        public void DisplayGroupConversation(int GroupId)
        {
            using (var dbContext = new DataBaseDbContext())
            {
                var gr = dbContext.MessagesGroups.Where(m => m.MessageGroupID == GroupId).ToList();

                foreach (var gritem in gr)
                {
                    var chatItem = new ChatItem { Message = gritem.GroupMessage };
                    AddItem(chatItem);
                }
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class ChatItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string message;
        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                OnPropertyChanged(nameof(Message));
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
