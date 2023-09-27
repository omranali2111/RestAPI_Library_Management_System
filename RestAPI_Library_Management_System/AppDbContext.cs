using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RestAPI_Library_Management_System
{
    public class AppDbContextDbContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer("Data Source=(local);Initial Catalog=EFCoreLibrary; Integrated Security=true; TrustServerCertificate=True");
        }
    }
    
    
}
