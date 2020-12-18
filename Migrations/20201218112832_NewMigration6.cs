using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace toDoList.Migrations
{
    public partial class NewMigration6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cLabs",
                columns: table => new
                {
                    IdCLab = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpresaSageId = table.Column<int>(type: "int", nullable: false),
                    Ano = table.Column<short>(type: "smallint", nullable: false),
                    DataLancamento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InputFilePAth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OutputFilePAth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserID = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cLabs", x => x.IdCLab);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cLabs");
        }
    }
}
