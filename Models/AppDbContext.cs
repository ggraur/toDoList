using Microsoft.EntityFrameworkCore;
using System;
using toDoClassLibrary;
using toDoList.Models;

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
            modelBuilder.Seed();
        }
    }
}
