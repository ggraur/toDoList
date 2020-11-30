using Microsoft.EntityFrameworkCore;
using System;
using toDoClassLibrary;
using toDoList.Models;
using toDoList.ViewModels;

namespace toDoList
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            :base(options) 
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<ToDoTask> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seed();
        }

        public DbSet<toDoClassLibrary.ToDoTask> Task { get; set; }

        public DbSet<toDoList.ViewModels.TaskCreateViewModel> TaskCreateViewModel { get; set; }
    }
}
