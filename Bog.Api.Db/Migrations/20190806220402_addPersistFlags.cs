using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bog.Api.Db.Migrations
{
    public partial class addPersistFlags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BlobUrl",
                table: "EntryMedia",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Persisted",
                table: "EntryMedia",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BlobUrl",
                table: "EntryContents",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Persisted",
                table: "EntryContents",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlobUrl",
                table: "EntryMedia");

            migrationBuilder.DropColumn(
                name: "Persisted",
                table: "EntryMedia");

            migrationBuilder.DropColumn(
                name: "BlobUrl",
                table: "EntryContents");

            migrationBuilder.DropColumn(
                name: "Persisted",
                table: "EntryContents");
        }
    }
}
