using Microsoft.EntityFrameworkCore.Migrations;

namespace toDoList.Migrations
{
    public partial class MyFirst1Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    UserPass = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserRole = table.Column<int>(type: "int", nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhotoPath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "PhotoPath", "UserEmail", "UserName", "UserPass", "UserRole" },
                values: new object[,]
                {
                    { 1, null, "mary@gmail.com", "Mary", "maryPass", 1 },
                    { 2, null, "Karl@gmail.com", "Karl", "karlPass", 2 },
                    { 3, null, "Eric@gmail.com", "Eric", "ericPass", 2 },
                    { 4, null, "Jorge@gmail.com", "Jorge", "jorgePass", 0 },
                    { 5, null, "Ann@gmail.com", "Ann", "annPass", 3 },
                    { 6, null, "Annette@gmail.com", "Annette", "annPass", 3 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
