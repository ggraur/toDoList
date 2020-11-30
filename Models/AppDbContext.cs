using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using System;
using toDoClassLibrary;
using toDoList.Models;
using toDoList.ViewModels;

namespace toDoList
{
    public class AppDbContext :IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            :base(options) 
        {

        }

 #pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        public DbSet<User> Users { get; set; }
 #pragma warning restore CS0114 // Member hides inherited member; missing override keyword
       public DbSet<ToDoTask> Tasks { get; set; }
         public DbSet<ToDoList> ToDoLists { get; set; }
        //public DbSet<toDoClassLibrary.ToDoTask> Task { get; set; }

        //public DbSet<toDoList.ViewModels.TaskCreateViewModel> TaskCreateViewModel { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<AppDbContext>()
            //   .HasKey();

            base.OnModelCreating(modelBuilder);
            modelBuilder.Seed();
        }

 
    }
}
