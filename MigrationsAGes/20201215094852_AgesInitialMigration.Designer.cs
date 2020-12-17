﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using toDoList.Models;

namespace toDoList.MigrationsAGes
{
    [DbContext(typeof(AGesContext))]
    [Migration("20201215094852_AgesInitialMigration")]
    partial class AgesInitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("toDoList.Models.EmpDat", b =>
                {
                    b.Property<string>("Cemp")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(25)
                        .IsUnicode(false)
                        .HasColumnType("varchar(25)")
                        .HasColumnName("CEmp")
                        .HasDefaultValueSql("('')");

                    b.Property<string>("Capl")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(8)
                        .IsUnicode(false)
                        .HasColumnType("varchar(8)")
                        .HasColumnName("CApl")
                        .HasDefaultValueSql("('')");

                    b.Property<short>("AnoIn")
                        .HasColumnType("smallint");

                    b.Property<short>("AnoFi")
                        .HasColumnType("smallint");

                    b.Property<string>("DataDir")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasDefaultValueSql("('')");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(1)
                        .IsUnicode(false)
                        .HasColumnType("varchar(1)")
                        .HasDefaultValueSql("('')");

                    b.Property<string>("Server")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)")
                        .HasDefaultValueSql("('')");

                    b.Property<string>("SharedDir")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasDefaultValueSql("('')");

                    b.HasKey("Cemp", "Capl", "AnoIn", "AnoFi")
                        .IsClustered(false);

                    b.ToTable("EmpDat");
                });

            modelBuilder.Entity("toDoList.Models.EmprViewModel", b =>
                {
                    b.Property<string>("Cemp")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(25)
                        .IsUnicode(false)
                        .HasColumnType("varchar(25)")
                        .HasColumnName("CEmp")
                        .HasDefaultValueSql("('')");

                    b.Property<string>("Actividade")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(40)
                        .IsUnicode(false)
                        .HasColumnType("varchar(40)")
                        .HasDefaultValueSql("('')");

                    b.Property<string>("Cae")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(5)
                        .IsUnicode(false)
                        .HasColumnType("varchar(5)")
                        .HasColumnName("CAE")
                        .HasDefaultValueSql("('')");

                    b.Property<decimal>("CapSocial")
                        .HasColumnType("money");

                    b.Property<string>("CodPostal")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasDefaultValueSql("('')");

                    b.Property<string>("Concelho")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasDefaultValueSql("('')");

                    b.Property<string>("ConsRegC")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)")
                        .HasDefaultValueSql("('')");

                    b.Property<string>("Distrito")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasDefaultValueSql("('')");

                    b.Property<string>("Email")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasDefaultValueSql("('')");

                    b.Property<string>("Fax")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(15)
                        .IsUnicode(false)
                        .HasColumnType("varchar(15)")
                        .HasDefaultValueSql("('')");

                    b.Property<string>("Freguesia")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasDefaultValueSql("('')");

                    b.Property<bool>("IsSelected")
                        .HasColumnType("bit");

                    b.Property<string>("Localidade")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasDefaultValueSql("('')");

                    b.Property<string>("Matriculado")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)")
                        .HasDefaultValueSql("('')");

                    b.Property<string>("Morada")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasDefaultValueSql("('')");

                    b.Property<string>("Ncontrib")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(12)
                        .IsUnicode(false)
                        .HasColumnType("varchar(12)")
                        .HasColumnName("NContrib")
                        .HasDefaultValueSql("('')");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)")
                        .HasDefaultValueSql("('')");

                    b.Property<short>("RfinancasC")
                        .HasColumnType("smallint")
                        .HasColumnName("RFinancasC");

                    b.Property<string>("RfinancasD")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("RFinancasD")
                        .HasDefaultValueSql("('')");

                    b.Property<string>("Sede")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(1)
                        .IsUnicode(false)
                        .HasColumnType("varchar(1)")
                        .HasDefaultValueSql("('')");

                    b.Property<string>("Telefone")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(15)
                        .IsUnicode(false)
                        .HasColumnType("varchar(15)")
                        .HasDefaultValueSql("('')");

                    b.HasKey("Cemp")
                        .IsClustered(false);

                    b.ToTable("Emprs");
                });

            modelBuilder.Entity("toDoList.Models.OpApl", b =>
                {
                    b.Property<string>("Coper")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(25)
                        .IsUnicode(false)
                        .HasColumnType("varchar(25)")
                        .HasColumnName("COper")
                        .HasDefaultValueSql("('')");

                    b.Property<string>("Capl")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(8)
                        .IsUnicode(false)
                        .HasColumnType("varchar(8)")
                        .HasColumnName("CApl")
                        .HasDefaultValueSql("('')");

                    b.HasKey("Coper", "Capl")
                        .IsClustered(false);

                    b.ToTable("OpApl");
                });

            modelBuilder.Entity("toDoList.Models.OpEmp", b =>
                {
                    b.Property<string>("Coper")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(25)
                        .IsUnicode(false)
                        .HasColumnType("varchar(25)")
                        .HasColumnName("COper")
                        .HasDefaultValueSql("('')");

                    b.Property<string>("Cemp")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(25)
                        .IsUnicode(false)
                        .HasColumnType("varchar(25)")
                        .HasColumnName("CEmp")
                        .HasDefaultValueSql("('')");

                    b.HasKey("Coper", "Cemp")
                        .IsClustered(false);

                    b.ToTable("OpEmp");
                });

            modelBuilder.Entity("toDoList.Models.OpPer", b =>
                {
                    b.Property<string>("Coper")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(25)
                        .IsUnicode(false)
                        .HasColumnType("varchar(25)")
                        .HasColumnName("COper")
                        .HasDefaultValueSql("('')");

                    b.Property<string>("Cper")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(15)
                        .IsUnicode(false)
                        .HasColumnType("varchar(15)")
                        .HasColumnName("CPer")
                        .HasDefaultValueSql("('')");

                    b.Property<bool>("Valor")
                        .HasColumnType("bit");

                    b.HasKey("Coper", "Cper")
                        .IsClustered(false);

                    b.ToTable("OpPer");
                });

            modelBuilder.Entity("toDoList.Models.Oper", b =>
                {
                    b.Property<string>("Coper")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(25)
                        .IsUnicode(false)
                        .HasColumnType("varchar(25)")
                        .HasColumnName("COper")
                        .HasDefaultValueSql("('')");

                    b.Property<string>("Email")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)")
                        .HasDefaultValueSql("('')");

                    b.Property<bool>("EpassForte")
                        .HasColumnType("bit")
                        .HasColumnName("EPassForte");

                    b.Property<bool>("Inact")
                        .HasColumnType("bit");

                    b.Property<byte>("Nivel")
                        .HasColumnType("tinyint");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)")
                        .HasDefaultValueSql("('')");

                    b.Property<string>("PassE")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)")
                        .HasDefaultValueSql("('')");

                    b.HasKey("Coper")
                        .IsClustered(false);

                    b.ToTable("Opers");
                });
#pragma warning restore 612, 618
        }
    }
}
