using Microsoft.EntityFrameworkCore;
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
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Rooms)
                .WithMany(r => r.Users);

            modelBuilder.Entity<Room>()
                .HasMany(r => r.Messages)
                .WithOne(m => m.ToRoom)
                .HasForeignKey(m => m.ToRoomId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Messages)
                .WithOne(m => m.FromUser)
                .HasForeignKey(m => m.FromUserId);
        }
    }
}
