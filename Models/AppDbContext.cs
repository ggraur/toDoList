using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
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

 
        public DbSet<User> Users { get; set; }
  
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
            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
        //public DbSet<toDoClassLibrary.ToDoTask> Task { get; set; }

        //public DbSet<toDoList.ViewModels.TaskCreateViewModel> TaskCreateViewModel { get; set; }

        public DbSet<toDoList.ViewModels.TasksViewModel> TasksViewModel { get; set; }

 
    }
}
