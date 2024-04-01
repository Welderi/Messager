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
                //Database.EnsureDeleted();
                Database.EnsureCreated();
            }
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=BigDBMessager;Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

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
                .HasForeignKey(gm => gm.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GroupMembership>()
                .HasOne(gm => gm.group)
                .WithMany()
                .HasForeignKey(gm => gm.GroupID)
                .OnDelete(DeleteBehavior.Restrict);
        }
        public void LoadInfo()
        {
            try
            {
                var sender = new User { Name = "John", Email = "john@example.com", Password = "password" };
                var recipient = new User { Name = "Alice", Email = "alice@example.com", Password = "password" };
                Users.AddRange(sender, recipient);
                SaveChanges();

                Contacts.Add(new Contact { User = sender });
                SaveChanges();

                Messages.Add(new Message { Sender = sender, Recipient = recipient, Content = "Hello, world!", Time = DateTime.Now });
                SaveChanges();

                var group = new Group { Name = "Test Group", Description = "This is a test group", CreationDate = DateTime.Now, Creator = sender };
                Groups.Add(group);
                SaveChanges();

                var groupMembership = new GroupMembership { user = recipient, group = group, Role = 1, JoinDate = DateTime.Now };
                GroupMemberships.Add(groupMembership);
                SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                MessageBox.Show($"An error occurred while updating the entries: {ex.Message}");
                if (ex.InnerException != null)
                {
                    MessageBox.Show($"Inner exception: {ex.InnerException.Message}");
                }
            }
        }
    }
}
