using Microsoft.EntityFrameworkCore.Migrations;

namespace toDoList.Migrations
{
    public partial class MyFirst2Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "UserEmail", "UserName", "UserPass", "UserRole" },
                values: new object[] { 1, "mary@gmail.com", "Mary", "maryPass", 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 1);
        }
    }
}
