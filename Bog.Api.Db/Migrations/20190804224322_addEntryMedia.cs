using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bog.Api.Db.Migrations
{
    public partial class addEntryMedia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EntryMedia",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EntryContentId = table.Column<Guid>(nullable: false),
                    FileName = table.Column<string>(nullable: false),
                    ContentType = table.Column<string>(nullable: false),
                    BlobFileName = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTimeOffset>(nullable: false),
                    MD5Base64Hash = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntryMedia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntryMedia_EntryContents_EntryContentId",
                        column: x => x.EntryContentId,
                        principalTable: "EntryContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntryMedia_EntryContentId",
                table: "EntryMedia",
                column: "EntryContentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntryMedia");
        }
    }
}
