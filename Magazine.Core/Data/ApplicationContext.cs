using Microsoft.EntityFrameworkCore;
using Magazine.Core.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Magazine.Core.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options):base(options)
        {
            //Database.EnsureCreated();
            Database.Migrate();
        }
    }
}

