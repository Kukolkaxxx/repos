using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using test.Models;

namespace test.Data
{
    public class ConncetionDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Reader> Readers { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void ConnectionString(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=(localdb)\MSSQLLocalDB;Initial Catalog=LibraryDB;Integrated Security=True;"
                );
        }
        protected override void ModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Reader>()
                .Property(b => b.FullName)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
