using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.API.Migrations
{
    /// <inheritdoc />
#pragma warning disable CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
    public partial class imgartist : Migration
#pragma warning restore CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MusicCatalogs_Presentations_PresentationId",
                table: "MusicCatalogs");

            migrationBuilder.CreateTable(
                name: "ImageArtists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArtistId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageArtists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImageArtists_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImageArtists_ArtistId",
                table: "ImageArtists",
                column: "ArtistId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MusicCatalogs_Presentations_PresentationId",
                table: "MusicCatalogs",
                column: "PresentationId",
                principalTable: "Presentations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MusicCatalogs_Presentations_PresentationId",
                table: "MusicCatalogs");

            migrationBuilder.DropTable(
                name: "ImageArtists");

            migrationBuilder.AddForeignKey(
                name: "FK_MusicCatalogs_Presentations_PresentationId",
                table: "MusicCatalogs",
                column: "PresentationId",
                principalTable: "Presentations",
                principalColumn: "Id");
        }
    }
}
