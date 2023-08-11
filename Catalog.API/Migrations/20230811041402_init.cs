using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.API.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Formats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Formats", x => x.Id);
                });

            migrationBuilder.Sql("INSERT INTO Formats (Name) VALUES ('Vinyl')");
            migrationBuilder.Sql("INSERT INTO Formats (Name) VALUES ('Cassette')");
            migrationBuilder.Sql("INSERT INTO Formats (Name) VALUES ('CD')");

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.Sql("INSERT INTO Genres (Name) VALUES ('Rock')");
            migrationBuilder.Sql("INSERT INTO Genres (Name) VALUES ('Rock/Psicodelico')");
            migrationBuilder.Sql("INSERT INTO Genres (Name) VALUES ('Jazz')");
            migrationBuilder.Sql("INSERT INTO Genres (Name) VALUES ('Blues')");
            migrationBuilder.Sql("INSERT INTO Genres (Name) VALUES ('Pop')");

            migrationBuilder.CreateTable(
                name: "Presentations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormatId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Presentations", x => x.Id);
                });

            migrationBuilder.Sql("INSERT INTO Presentations (Name, FormatId) VALUES ('12', 1)");
            migrationBuilder.Sql("INSERT INTO Presentations (Name, FormatId) VALUES ('12 Gatefold 2LP', 1)");
            migrationBuilder.Sql("INSERT INTO Presentations (Name, FormatId) VALUES ('7', 1)");
            migrationBuilder.Sql("INSERT INTO Presentations (Name, FormatId) VALUES ('Metal', 2)");
            migrationBuilder.Sql("INSERT INTO Presentations (Name, FormatId) VALUES ('CD Doble', 3)");    

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

            migrationBuilder.CreateTable(
                name: "MusicCatalogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArtistId = table.Column<int>(type: "int", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false),
                    FormatId = table.Column<int>(type: "int", nullable: false),
                    PresentationId = table.Column<int>(type: "int", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    StatusCover = table.Column<int>(type: "int", nullable: false),
                    StatusGeneral = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Matrix = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MusicCatalogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MusicCatalogs_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MusicCatalogs_Formats_FormatId",
                        column: x => x.FormatId,
                        principalTable: "Formats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MusicCatalogs_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MusicCatalogs_Presentations_PresentationId",
                        column: x => x.PresentationId,
                        principalTable: "Presentations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImagesCatalog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MusicCatalogId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImagesCatalog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImagesCatalog_MusicCatalogs_MusicCatalogId",
                        column: x => x.MusicCatalogId,
                        principalTable: "MusicCatalogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImageArtists_ArtistId",
                table: "ImageArtists",
                column: "ArtistId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImagesCatalog_MusicCatalogId",
                table: "ImagesCatalog",
                column: "MusicCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_MusicCatalogs_ArtistId",
                table: "MusicCatalogs",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_MusicCatalogs_FormatId",
                table: "MusicCatalogs",
                column: "FormatId");

            migrationBuilder.CreateIndex(
                name: "IX_MusicCatalogs_GenreId",
                table: "MusicCatalogs",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_MusicCatalogs_PresentationId",
                table: "MusicCatalogs",
                column: "PresentationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImageArtists");

            migrationBuilder.DropTable(
                name: "ImagesCatalog");

            migrationBuilder.DropTable(
                name: "MusicCatalogs");

            migrationBuilder.DropTable(
                name: "Artists");

            migrationBuilder.DropTable(
                name: "Formats");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Presentations");
        }
    }
}
