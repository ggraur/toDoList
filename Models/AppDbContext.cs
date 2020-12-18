
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using toDoClassLibrary;
using toDoList.Models;
using toDoList.ViewModels;


namespace toDoList
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            :base(options) 
        {

        }

       // public DbSet<MyUser> MyUsers { get; set; }

        public DbSet<AppUserAddress> UserAddress { get; set; }
  
       //public DbSet<ToDoTask> Tasks { get; set; }
       //public DbSet<ToDoList> ToDoLists { get; set; }

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
            modelBuilder.Entity<mapView_UtilzadoresEmpresa>(
                x => 
                {
                    x.HasNoKey();
                    x.ToView("db_vw_UtilizadoresEmpresa");
                    //x.Property(v => v.EmpresaId).HasColumnName("EmpresaId");
                    
                });
            //object p = modelBuilder.Query<mapView_UtilzadoresEmpresa>().ToView("db_vw_UtilizadoresEmpresa");

        }

        //public DbSet<AddTask_To_ToDoList> AddTask_To_ToDoList { get; set; }
        public DbSet<EditRoleViewModel> EditRoleViewModel { get; set; }
        public DbSet<ResetPasswordViewModel> ResetPasswordViewModel { get; set; }
        public DbSet<ForgotPasswordViewModel> ForgotPasswordViewModel { get; set; }
        public DbSet<ConConfigViewModel> ConConfigViewModel { get; set; }
        public DbSet<EmpresasViewModel> EmpresasViewModel { get; set; }
        public DbSet<EmpresaUtilizadoresViewModel> EmpresaUtilizadores { get; set; }
        public DbSet<mapView_UtilzadoresEmpresa> View_UtilzadoresEmpresa { get; set; }
        public DbSet<CLab> cLabs { get; set;}


    }
}
