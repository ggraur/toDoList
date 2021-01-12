using Microsoft.EntityFrameworkCore.Migrations;

namespace toDoList.Migrations
{
    public partial class NewMigration7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InputFilePAth",
                table: "cLabs",
                newName: "InputFilePath");

            migrationBuilder.AlterColumn<string>(
                name: "InputFilePath",
                table: "cLabs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "AnoEmpresa",
                columns: table => new
                {
                    AnoEmpresaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpresaID = table.Column<int>(type: "int", nullable: false),
                    CodeEmpresa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodeAplicacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnoIn = table.Column<short>(type: "smallint", nullable: false),
                    AnoFi = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnoEmpresa", x => x.AnoEmpresaID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnoEmpresa");

            migrationBuilder.RenameColumn(
                name: "InputFilePath",
                table: "cLabs",
                newName: "InputFilePAth");

            migrationBuilder.AlterColumn<string>(
                name: "InputFilePAth",
                table: "cLabs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
