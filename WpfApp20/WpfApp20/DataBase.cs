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

namespace WpfApp20
{
    public class DataBaseDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupMembership> GroupMemberships { get; set; }
        public DataBaseDbContext()
        {
            if (!Database.CanConnect())
            {
                Database.EnsureDeleted();
                Database.EnsureCreated();
            }
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=BigDBMessager2;Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>()
                .HasIndex(c => new { c.ConcreteUserID, c.UserID })
                .IsUnique();

            modelBuilder.Entity<Contact>()
                .HasOne(m => m.ConcreteUser)
                .WithMany()
                .HasForeignKey(m => m.ConcreteUserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Contact>()
                .HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Recipient)
                .WithMany()
                .HasForeignKey(m => m.RecipientID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Group>()
                .HasOne(g => g.Creator)
                .WithMany()
                .HasForeignKey(g => g.CreatorID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GroupMembership>()
                .HasOne(gm => gm.user)
                .WithMany()
                .HasForeignKey(gm => gm.UserGMID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GroupMembership>()
                .HasOne(gm => gm.group)
                .WithMany()
                .HasForeignKey(gm => gm.GroupGMID)
                .OnDelete(DeleteBehavior.Restrict);
        }

        //  User
        public void AddToUsers(string name, string pass, string email)
        {
            Users.Add(new User { Name = name, Password = pass, Email = email});
            SaveChanges();
        }
        public void DeleteFromUsers(int id)
        {
            var userToRemove = Users.FirstOrDefault(u => u.UserID == id);
            if (userToRemove != null)
            {
                Users.Remove(userToRemove);
                SaveChanges();
            }
        }
        public bool CheckUserExists(string login)
        {
            return Users.Any(u => u.Name == login);
        }
        public bool CheckEmailExists(string email)
        {
            return Users.Any(u => u.Email == email);
        }
        public bool CheckUserPassword(string name, string pass)
        {
            var user = Users.FirstOrDefault(u => u.Name == name);

            if (user != null && user.Password == pass)
            {
                return true; 
            }
            else
            {
                return false;
            }
        }
        public int GetId(string login)
        {
            var user = Users.FirstOrDefault(u => u.Name == login);
            return user.UserID;
        }
        //  Contact
        public void AddToContacts(int concreteuserID, int userID)
        {
            Contacts.Add(new Contact { UserID = userID, ConcreteUserID = concreteuserID});
            SaveChanges();
        }
        public void DeleteFromContacts(int id)
        {
            var contactToRemove = Contacts.FirstOrDefault(u => u.ContactID == id);
            if (contactToRemove != null)
            {
                Contacts.Remove(contactToRemove);
                SaveChanges();
            }
        }

        //  Message
        public void AddToMessages(int senderID, int recipientID, string content, bool isRead, DateTime time)
        {
            Messages.Add(new Message { SenderID = senderID, RecipientID = recipientID, 
                                       Content = content, IsRead = isRead});
            SaveChanges();
        }
        public void DeleteFromMessages(int id)
        {
            var messageToRemove = Messages.FirstOrDefault(u => u.MessageID == id);
            if (messageToRemove != null)
            {
                Messages.Remove(messageToRemove);
                SaveChanges();
            }
        }

        //  Group
        public void AddToGroups(string name, string desc, DateTime time, int creatorID)
        {
            Groups.Add(new Group { Name = name, Description = desc, 
                                   CreationDate = time, CreatorID = creatorID});
            SaveChanges();
        }
        public void DeleteFromGroups(int id)
        {
            var groupToRemove = Groups.FirstOrDefault(u => u.GroupID == id);
            if (groupToRemove != null)
            {
                Groups.Remove(groupToRemove);
                SaveChanges();
            }
        }

        // GM
        public void AddToGroupMemberships(int userGMID, int groupGMID, string role, DateTime time)
        {
            GroupMemberships.Add(new GroupMembership { UserGMID = userGMID, GroupGMID = groupGMID, 
                                                       Role = role, JoinDate = time});
            SaveChanges();
        }
        public void DeleteFromGroupMemberships(int id)
        {
            var GMToRemove = GroupMemberships.FirstOrDefault(u => u.MemberID == id);
            if (GMToRemove != null)
            {
                GroupMemberships.Remove(GMToRemove);
                SaveChanges();
            }
        }
    }
}
