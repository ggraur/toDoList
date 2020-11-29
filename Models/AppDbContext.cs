using Microsoft.EntityFrameworkCore;
using System;
using toDoClassLibrary;
namespace toDoList
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            :base(options) 
        {

        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                     new User() { UserID = 1, UserName = "Mary", UserPass = "maryPass", UserRole = UserRoleEnum.Administrator, UserEmail = "mary@gmail.com" }
                );
        }
    }
}
