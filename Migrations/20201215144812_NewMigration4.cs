using Microsoft.EntityFrameworkCore.Migrations;

namespace toDoList.Migrations
{
    public partial class NewMigration4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdCabContabilidade",
                table: "EmpresasViewModel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "isCabContabilidade",
                table: "EmpresasViewModel",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdCabContabilidade",
                table: "EmpresasViewModel");

            migrationBuilder.DropColumn(
                name: "isCabContabilidade",
                table: "EmpresasViewModel");
        }
    }
}
