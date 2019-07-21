using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bog.Api.Db.Migrations
{
    public partial class removefieldsentrycontent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "EntryContents");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "EntryContents");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Deleted",
                table: "EntryContents",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Updated",
                table: "EntryContents",
                nullable: true);
        }
    }
}
