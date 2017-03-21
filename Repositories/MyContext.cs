using System.Configuration;
using BargainBot.Model;
using Microsoft.EntityFrameworkCore;

namespace BargainBot.Repositories
{
    public class MyContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Deal> Deals { get; set; }
        private readonly string _mysqlConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(_mysqlConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Deal>()
                .HasOne(u => u.User)
                .WithMany(d => d.Deals);
        }
    }
}