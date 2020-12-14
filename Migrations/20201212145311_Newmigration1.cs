using System;
using System.IO;
using Microsoft.EntityFrameworkCore.Migrations;
using toDoList.Models;

namespace toDoList.Migrations
{
    public partial class Newmigration1 : Migration
    {
        private const string MIGRATION_SQL_SCRIPT_FILE_NAME = @"Migrations\20201212145311_Newmigration1.sql";

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddTask_To_ToDoList");

            migrationBuilder.DropTable(
                name: "MyUsers");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "ToDoLists");

            //https://stackoverflow.com/questions/58693964/create-database-views-using-ef-core-code-first-approach
            //https://stackoverflow.com/questions/45035754/how-to-run-migration-sql-script-using-entity-framework-core

            string sql = Path.Combine(AppContext.BaseDirectory, MIGRATION_SQL_SCRIPT_FILE_NAME);
            migrationBuilder.Sql(File.ReadAllText(sql));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AddTask_To_ToDoList",
                columns: table => new
                {
                    ListTaskID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IDExecutor = table.Column<int>(type: "int", nullable: false),
                    IsChecked = table.Column<bool>(type: "bit", nullable: false),
                    TaskDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaskID = table.Column<int>(type: "int", nullable: false),
                    TaskName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaskStatus = table.Column<int>(type: "int", nullable: false),
                    ToDoListID = table.Column<int>(type: "int", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddTask_To_ToDoList", x => x.ListTaskID);
                });

            migrationBuilder.CreateTable(
                name: "MyUsers",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhotoPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    UserPass = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserRole = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyUsers", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    TaskID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskActive = table.Column<int>(type: "int", nullable: false),
                    TaskDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaskName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.TaskID);
                });

            migrationBuilder.CreateTable(
                name: "ToDoLists",
                columns: table => new
                {
                    ToDoListID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedToDoListDatetime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinalizationDatetime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IDCreator = table.Column<int>(type: "int", nullable: false),
                    IDExecutor = table.Column<int>(type: "int", nullable: false),
                    ToDoListName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserIDCreator = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserIDExecutor = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDoLists", x => x.ToDoListID);
                });
        }
    }
}
