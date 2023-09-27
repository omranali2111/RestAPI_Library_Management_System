using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RestAPI_Library_Management_System.models;

namespace RestAPI_Library_Management_System
{
    public class AppDbContextDbContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer("Data Source=(local);Initial Catalog=EFCoreLibrary; Integrated Security=true; TrustServerCertificate=True");
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Patron> Patrons { get; set; }
        public DbSet<BorrowingHistory> BorrowingHistories { get; set; }
    }
    
    
}
