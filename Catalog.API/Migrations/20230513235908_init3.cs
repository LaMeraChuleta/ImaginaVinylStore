using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.API.Migrations
{
    /// <inheritdoc />
    public partial class init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Presentations_Formats_FormatId",
                table: "Presentations");

            migrationBuilder.DropIndex(
                name: "IX_Presentations_FormatId",
                table: "Presentations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Presentations_FormatId",
                table: "Presentations",
                column: "FormatId");

            migrationBuilder.AddForeignKey(
                name: "FK_Presentations_Formats_FormatId",
                table: "Presentations",
                column: "FormatId",
                principalTable: "Formats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
