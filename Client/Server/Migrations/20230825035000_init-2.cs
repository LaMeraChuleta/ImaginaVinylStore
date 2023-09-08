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
            migrationBuilder.DropTable(
                name: "CarItem");

            migrationBuilder.CreateTable(
                name: "ShopCart",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MusicCatalogId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopCart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopCart_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShopCart_MusicCatalog_MusicCatalogId",
                        column: x => x.MusicCatalogId,
                        principalTable: "MusicCatalog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShopCart_ApplicationUserId",
                table: "ShopCart",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopCart_MusicCatalogId",
                table: "ShopCart",
                column: "MusicCatalogId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShopCart");

            migrationBuilder.CreateTable(
                name: "CarItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MusicCatalogId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId1 = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UnitPrice = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarItem_AspNetUsers_ApplicationUserId1",
                        column: x => x.ApplicationUserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CarItem_MusicCatalog_MusicCatalogId",
                        column: x => x.MusicCatalogId,
                        principalTable: "MusicCatalog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarItem_ApplicationUserId1",
                table: "CarItem",
                column: "ApplicationUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_CarItem_MusicCatalogId",
                table: "CarItem",
                column: "MusicCatalogId");
        }
    }
}
