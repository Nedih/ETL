using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETL.Models;
using Microsoft.EntityFrameworkCore;

namespace ETL.EF
{
    public class AppDbContext : DbContext
    {
        public DbSet<Record> Records { get; set; } 

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=ETLDatabase;Trusted_Connection=True;");
        }
    }
}
