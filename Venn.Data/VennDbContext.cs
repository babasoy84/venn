using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venn.Models.Models.Concretes;

namespace Venn.Data
{
    public class VennDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=VennDb;Integrated Security=True;");
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        { 
            modelBuilder.Entity<User>()
                .HasMany(u => u.Messages)
                .WithOne(m => m.FromUser)
                .HasForeignKey(m => m.FromUserId)
                .OnDelete(DeleteBehavior.Restrict); 
            
            modelBuilder.Entity<User>()
                .HasMany(u => u.Notifications)
                .WithOne(n => n.ToUser)
                .HasForeignKey(n => n.ToUserId)
                .OnDelete(DeleteBehavior.Restrict); 
            
            modelBuilder.Entity<User>()
                .HasMany(u => u.Contacts)
                .WithOne(fs => fs.User1)
                .HasForeignKey(fs => fs.User1Id)
                .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}
