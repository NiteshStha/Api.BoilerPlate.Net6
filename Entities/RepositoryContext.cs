using Entities.Models;
using Utility;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seeding User Table
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                FirstName = "Admin",
                LastName = "Admin",
                Username = "Admin",
                Email = "admin@gmail.com",
                Password = PasswordHelper.Hash("admin"),
                Role = Role.Admin
            });
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 2,
                FirstName = "Nitesh",
                LastName = "Shrestha",
                Username = "nitesh",
                Email = "nitesh@gmail.com",
                Password = PasswordHelper.Hash("nitesh"),
                Role = Role.User
            });
        }
    }
}