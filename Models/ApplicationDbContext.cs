using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoList.ViewModels;

namespace toDoList.Models
{
    public class ApplicationDbContext : DbContext
    {
        
        private readonly IConfiguration config;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            config = new ConfigurationBuilder()
            .SetBasePath("../appsettings.json")
            .AddJsonFile("appsettings.json")
            .Build();
        }

        //private static DbContextOptions GetOptions(string connectionString)
        //{
        //    return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString).Options;
        //}
        //public virtual DbSet<EmpresasViewModel>GabineteContabilidade { get; set; }
        //public virtual DbSet<DadosEmpresaViewModel> DadosEmpresaViewModel { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<mapView_GabContabilidade>(
            //   x =>
            //   {
            //       x.HasNoKey();
            //       x.ToView("db_vw_GabContabilidade");
            //    });
            //modelBuilder.Entity<mapView_EmpresasGabContabilidade>(
            //   x =>
            //   {
            //       x.HasNoKey();
            //       x.ToView("db_vw_EmpresasGabContabilidade");
            //   });
            //modelBuilder.Entity<mapView_EmpresasGabContabilidadeAno>(
            //   x =>
            //   {
            //       x.HasNoKey();
            //       x.ToView("db_vw_EmpresasGabContabilidadeAno");
            //   });

            //db_vw_EmpresasGabContabilidadeAno
            //db_vw_EmpresasGabContabilidade
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(config.GetConnectionString("ApplicationDbContext"));
        }


    }
}
