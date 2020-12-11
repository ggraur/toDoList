using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace toDoList.Migrations
{
    public partial class NewMigration13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResetPasswordViewModel");

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetLinkConfirmationDate",
                table: "ForgotPasswordViewModel",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetLinkCreatedTime",
                table: "ForgotPasswordViewModel",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetLinkValidity",
                table: "ForgotPasswordViewModel",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "ForgotPasswordViewModel",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResetLinkConfirmationDate",
                table: "ForgotPasswordViewModel");

            migrationBuilder.DropColumn(
                name: "ResetLinkCreatedTime",
                table: "ForgotPasswordViewModel");

            migrationBuilder.DropColumn(
                name: "ResetLinkValidity",
                table: "ForgotPasswordViewModel");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "ForgotPasswordViewModel");

            migrationBuilder.CreateTable(
                name: "ResetPasswordViewModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConfirmPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResetLinkConfirmationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResetLinkCreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResetLinkValidity = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResetPasswordViewModel", x => x.Id);
                });
        }
    }
}
