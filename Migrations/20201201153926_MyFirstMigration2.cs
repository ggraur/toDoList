using Microsoft.EntityFrameworkCore.Migrations;

namespace toDoList.Migrations
{
    public partial class MyFirstMigration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "PageTitle",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "TaskChecked",
                table: "Tasks");

            migrationBuilder.AddColumn<int>(
                name: "IDCreator",
                table: "ToDoLists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IDExecutor",
                table: "ToDoLists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AddTask_To_ToDoList",
                columns: table => new
                {
                    ListTaskID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ToDoListID = table.Column<int>(type: "int", nullable: false),
                    IsChecked = table.Column<bool>(type: "bit", nullable: false),
                    TaskID = table.Column<int>(type: "int", nullable: false),
                    TaskName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaskDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaskActive = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddTask_To_ToDoList", x => x.ListTaskID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddTask_To_ToDoList");

            migrationBuilder.DropColumn(
                name: "IDCreator",
                table: "ToDoLists");

            migrationBuilder.DropColumn(
                name: "IDExecutor",
                table: "ToDoLists");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PageTitle",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TaskChecked",
                table: "Tasks",
                type: "bit",
                nullable: true);
        }
    }
}
