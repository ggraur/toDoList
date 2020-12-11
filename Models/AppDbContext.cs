using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using toDoClassLibrary;
using toDoList.Models;
using toDoList.ViewModels;


namespace toDoList
{
    public class AppDbContext :  IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            :base(options) 
        {

        }

        public DbSet<MyUser> MyUsers { get; set; }

        public DbSet<AppUserAddress> UserAddress { get; set; }
  
       public DbSet<ToDoTask> Tasks { get; set; }
       public DbSet<ToDoList> ToDoLists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<AppDbContext>()
            //   .HasNoKey();
     
            base.OnModelCreating(modelBuilder);
            modelBuilder.Seed();
            
            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        public DbSet<AddTask_To_ToDoList> AddTask_To_ToDoList { get; set; }
        public DbSet<EditRoleViewModel> EditRoleViewModel { get; set; }
        public DbSet<ResetPasswordViewModel> ResetPasswordViewModel { get; set; }
        public DbSet<ForgotPasswordViewModel> ForgotPasswordViewModel { get; set; }


    }
}
