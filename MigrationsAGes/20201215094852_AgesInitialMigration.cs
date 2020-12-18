using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.IdentityModel.Protocols;
using toDoList.Models;

namespace toDoList.MigrationsAGes
{
    public partial class AgesInitialMigration : Migration
    {
        //private readonly AGesContext context;

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = @"
            CREATE OR ALTER VIEW [dbo].[vwEmpresaUtilizadores] AS
                    SELECT t1.CEmp [CodeEmpresa]  , t1.Nome [NomeEmpresa],  t1.NContrib [NIF],
                    t2.CApl[CodeApplicacao], t2.AnoIn, t2.AnoFi, 
                    t3.COper [Utilizador]
                    FROM   Emprs t1
                    left join EmpDat  t2 on t1.CEmp=t2.CEmp
                    Left join OpEmp t3 on t1.CEmp = t3.CEmp
                    group by t1.CEmp, t1.Nome,  t1.NContrib,t2.CApl, t2.AnoFi,t2.AnoIn,   t3.COper ";

            //EX: https://www.michalbialecki.com/2020/09/09/working-with-views-in-entity-framework-core-5/
            migrationBuilder.Sql(sql);
        }

        //protected override void Up(MigrationBuilder migrationBuilder)
        //{
        //    migrationBuilder.CreateTable(
        //        name: "EmpDat",
        //        columns: table => new
        //        {
        //            CEmp = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: false, defaultValueSql: "('')"),
        //            CApl = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false, defaultValueSql: "('')"),
        //            AnoIn = table.Column<short>(type: "smallint", nullable: false),
        //            AnoFi = table.Column<short>(type: "smallint", nullable: false),
        //            DataDir = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false, defaultValueSql: "('')"),
        //            Server = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false, defaultValueSql: "('')"),
        //            SharedDir = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false, defaultValueSql: "('')"),
        //            Estado = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: false, defaultValueSql: "('')")
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_EmpDat", x => new { x.CEmp, x.CApl, x.AnoIn, x.AnoFi })
        //                .Annotation("SqlServer:Clustered", false);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "Emprs",
        //        columns: table => new
        //        {
        //            CEmp = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: false, defaultValueSql: "('')"),
        //            IsSelected = table.Column<bool>(type: "bit", nullable: false),
        //            Nome = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false, defaultValueSql: "('')"),
        //            Morada = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValueSql: "('')"),
        //            Localidade = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValueSql: "('')"),
        //            CodPostal = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValueSql: "('')"),
        //            Distrito = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValueSql: "('')"),
        //            Concelho = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValueSql: "('')"),
        //            Freguesia = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValueSql: "('')"),
        //            Sede = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: false, defaultValueSql: "('')"),
        //            Telefone = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false, defaultValueSql: "('')"),
        //            Fax = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false, defaultValueSql: "('')"),
        //            Email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValueSql: "('')"),
        //            NContrib = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: false, defaultValueSql: "('')"),
        //            CapSocial = table.Column<decimal>(type: "money", nullable: false),
        //            ConsRegC = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false, defaultValueSql: "('')"),
        //            Matriculado = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false, defaultValueSql: "('')"),
        //            CAE = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: false, defaultValueSql: "('')"),
        //            Actividade = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false, defaultValueSql: "('')"),
        //            RFinancasC = table.Column<short>(type: "smallint", nullable: false),
        //            RFinancasD = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValueSql: "('')")
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Emprs", x => x.CEmp)
        //                .Annotation("SqlServer:Clustered", false);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "OpApl",
        //        columns: table => new
        //        {
        //            COper = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: false, defaultValueSql: "('')"),
        //            CApl = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false, defaultValueSql: "('')")
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_OpApl", x => new { x.COper, x.CApl })
        //                .Annotation("SqlServer:Clustered", false);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "OpEmp",
        //        columns: table => new
        //        {
        //            COper = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: false, defaultValueSql: "('')"),
        //            CEmp = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: false, defaultValueSql: "('')")
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_OpEmp", x => new { x.COper, x.CEmp })
        //                .Annotation("SqlServer:Clustered", false);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "Opers",
        //        columns: table => new
        //        {
        //            COper = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: false, defaultValueSql: "('')"),
        //            Nome = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false, defaultValueSql: "('')"),
        //            Nivel = table.Column<byte>(type: "tinyint", nullable: false),
        //            PassE = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false, defaultValueSql: "('')"),
        //            Email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false, defaultValueSql: "('')"),
        //            EPassForte = table.Column<bool>(type: "bit", nullable: false),
        //            Inact = table.Column<bool>(type: "bit", nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Opers", x => x.COper)
        //                .Annotation("SqlServer:Clustered", false);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "OpPer",
        //        columns: table => new
        //        {
        //            COper = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: false, defaultValueSql: "('')"),
        //            CPer = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false, defaultValueSql: "('')"),
        //            Valor = table.Column<bool>(type: "bit", nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_OpPer", x => new { x.COper, x.CPer })
        //                .Annotation("SqlServer:Clustered", false);
        //        });
        //}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW vwEmpresaUtilizadores");
        }
    }
}
