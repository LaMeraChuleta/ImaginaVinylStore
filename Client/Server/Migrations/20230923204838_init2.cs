using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Client.Server.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Discount",
                table: "MusicCatalog",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Discount",
                table: "AudioCatalog",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "MusicCatalog");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "AudioCatalog");
        }
    }
}
