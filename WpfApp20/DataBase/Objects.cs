using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataBase
{
    public class User : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        [Key]
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Contact> Contacts { get; set; }
        public List<Message> Messages { get; set; }
        public List<Group> Groups { get; set; }
        public List<GroupMembership> groupMemberships { get; set; }
    }
    public class Contact : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        [Key]
        public int ContactID { get; set; }

        public int ConcreteUserID { get; set; }
        [ForeignKey("ConcreteUserID")]
        public User ConcreteUser { get; set; }
        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public User User { get; set; }

    }
    public class Message : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        [Key]
        public int MessageID { get; set; }

        public int SenderID { get; set; }
        [ForeignKey("SenderID")]
        public User Sender { get; set; }

        public int RecipientID { get; set; }
        [ForeignKey("RecipientID")]
        public User Recipient { get; set; }

        public string Content { get; set; }

        public bool IsRead { get; set; }

        public DateTime Time { get; set; }
    }
    public class Group : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        [Key]
        public int GroupID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public int CreatorID { get; set; }
        [ForeignKey("CreatorID")]
        public User Creator { get; set; }
        public List<GroupMembership> Memberships { get; set; }
    }

    public class GroupMembership : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        [Key]
        public int MemberID { get; set; }
        public int UserGMID { get; set; }
        [ForeignKey("UserGMID")]
        public User user { get; set; }
        public int GroupGMID { get; set; }
        [ForeignKey("GroupGMID")]
        public Group group { get; set; }
        public string Role { get; set; }
        public DateTime JoinDate { get; set; }
    }
}
