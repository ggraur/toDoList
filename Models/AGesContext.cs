using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using toDoList.ViewModels;

#nullable disable

namespace toDoList.Models
{
    public partial class AGesContext : DbContext
    {
        //public AGesContext()
        //{
        //}

        public AGesContext(DbContextOptions<AGesContext> options)
            : base(options)
        {
        }
        
        public virtual DbSet<EmpDat> EmpDats { get; set; }
        
        public virtual DbSet<Empr> Emprs { get; set; }
        public virtual DbSet<OpApl> OpApls { get; set; }
        public virtual DbSet<OpEmp> OpEmps { get; set; }
        public virtual DbSet<OpPer> OpPers { get; set; }
        public virtual DbSet<Oper> Opers { get; set; }

        public virtual DbSet<AGesEmpresasUtilizadores> AGesEmpresasUtilizadores { get; set; }

        //public virtual DbSet<EmpDhist> EmpDhists { get; set; }
        //public virtual DbSet<CfgExter> CfgExters { get; set; }
        //public virtual DbSet<LocEn> LocEns { get; set; }
        //public virtual DbSet<MetaP> MetaPs { get; set; }
        //public virtual DbSet<ParEmp> ParEmps { get; set; }
        //public virtual DbSet<SageLicence> SageLicences { get; set; }
        //public virtual DbSet<Serv> Servs { get; set; }
        //public virtual DbSet<StdUtilizadore> StdUtilizadores { get; set; }
        //public virtual DbSet<StdVersao> StdVersaos { get; set; }
        //public virtual DbSet<TarefCb> TarefCbs { get; set; }
        //public virtual DbSet<TarefLn> TarefLns { get; set; }
        //public virtual DbSet<Versao> Versaos { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("server=.;database=AGes;Trusted_Connection=true;MultipleActiveResultSets=true;");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<AGesEmpresasUtilizadores>(empUtil => {
                    empUtil.HasNoKey();
                    empUtil.ToView("vwEmpresaUtilizadores");
                });
            modelBuilder.Entity<EmpDat>(entity =>
            {
                entity.HasKey(e => new { e.Cemp, e.Capl, e.AnoIn, e.AnoFi })
                    .IsClustered(false);

                entity.ToTable("EmpDat");

                entity.Property(e => e.Cemp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("CEmp")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Capl)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("CApl")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DataDir)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Server)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.SharedDir)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<EmprViewModel>(entity =>
            {
                entity.HasKey(e => e.Cemp)
                    .IsClustered(false);

                entity.Property(e => e.Cemp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("CEmp")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Actividade)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Cae)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("CAE")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CapSocial)
                    .HasColumnType("money")
                    .HasPrecision(3)
                    .HasConversion<decimal>();
                    

                entity.Property(e => e.CodPostal)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Concelho)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ConsRegC)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Distrito)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Fax)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Freguesia)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Localidade)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Matriculado)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Morada)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ncontrib)
                    .IsRequired()
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("NContrib")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RfinancasC).HasColumnName("RFinancasC");

                entity.Property(e => e.RfinancasD)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("RFinancasD")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Sede)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Telefone)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<OpApl>(entity =>
            {
                entity.HasKey(e => new { e.Coper, e.Capl })
                    .IsClustered(false);

                entity.ToTable("OpApl");

                entity.Property(e => e.Coper)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("COper")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Capl)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("CApl")
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<OpEmp>(entity =>
            {
                entity.HasKey(e => new { e.Coper, e.Cemp })
                    .IsClustered(false);

                entity.ToTable("OpEmp");

                entity.Property(e => e.Coper)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("COper")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Cemp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("CEmp")
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<OpPer>(entity =>
            {
                entity.HasKey(e => new { e.Coper, e.Cper })
                    .IsClustered(false);

                entity.ToTable("OpPer");

                entity.Property(e => e.Coper)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("COper")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Cper)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("CPer")
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<Oper>(entity =>
            {
                entity.HasKey(e => e.Coper)
                    .IsClustered(false);

                entity.Property(e => e.Coper)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("COper")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.EpassForte).HasColumnName("EPassForte");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PassE)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            OnModelCreatingPartial(modelBuilder);
        }

      

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public DbSet<toDoList.ViewModels.EmpresasViewModel> EmpresasViewModel { get; set; }
    }
}


//https://stackoverflow.com/questions/43767933/entity-framework-core-using-multiple-dbcontexts
//https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/providers?tabs=dotnet-core-cli
//https://docs.microsoft.com/en-us/ef/core/cli/dotnet
//https://github.com/dotnet/efcore/issues/9433
//https://www.codeproject.com/Articles/848111/Multi-Tenancy-System-With-Separate-Databases-in-MV
/*
 
 1.  dotnet ef migrations add init --context AgesContext --output-dir MigrationsAGes
 2.  dotnet ef database update 20201215094852_AgesInitialMigration --connection AgesContext
   
*/