using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestForItMonitoring.DatabaseModels;

namespace TestForItMonitoring.Context
{
    public class DataContext : DbContext
    {
        public DbSet<RequestModel> Requests { get; set; }

        public DataContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("ConnectionString");
        }
    }
}
