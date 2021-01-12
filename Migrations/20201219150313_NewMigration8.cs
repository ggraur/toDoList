using Microsoft.EntityFrameworkCore.Migrations;

namespace toDoList.Migrations
{
    public partial class NewMigration8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnoEmpresa");

            migrationBuilder.CreateTable(
                name: "DadosEmpresaImportada",
                columns: table => new
                {
                    DEmpresaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpresaID = table.Column<int>(type: "int", nullable: false),
                    CodeEmpresa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodeAplicacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnoIn = table.Column<short>(type: "smallint", nullable: false),
                    AnoFi = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DadosEmpresaImportada", x => x.DEmpresaID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DadosEmpresaImportada");

            migrationBuilder.CreateTable(
                name: "AnoEmpresa",
                columns: table => new
                {
                    AnoEmpresaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnoFi = table.Column<short>(type: "smallint", nullable: false),
                    AnoIn = table.Column<short>(type: "smallint", nullable: false),
                    CodeAplicacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodeEmpresa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmpresaID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnoEmpresa", x => x.AnoEmpresaID);
                });
        }
    }
}
