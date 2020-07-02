using Microsoft.EntityFrameworkCore.Migrations;

namespace Bog.Api.Db.Migrations
{
    public partial class addEntryContentConvertedUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConvertedBlobUrl",
                table: "EntryContents",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConvertedBlobUrl",
                table: "EntryContents");
        }
    }
}
