using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Client.Server.Migrations
{
    /// <inheritdoc />
    public partial class init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopCart_AudioCatalog_AudioCatalogId",
                table: "ShopCart");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopCart_MusicCatalog_MusicCatalogId",
                table: "ShopCart");

            migrationBuilder.AlterColumn<int>(
                name: "MusicCatalogId",
                table: "ShopCart",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AudioCatalogId",
                table: "ShopCart",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopCart_AudioCatalog_AudioCatalogId",
                table: "ShopCart",
                column: "AudioCatalogId",
                principalTable: "AudioCatalog",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopCart_MusicCatalog_MusicCatalogId",
                table: "ShopCart",
                column: "MusicCatalogId",
                principalTable: "MusicCatalog",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopCart_AudioCatalog_AudioCatalogId",
                table: "ShopCart");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopCart_MusicCatalog_MusicCatalogId",
                table: "ShopCart");

            migrationBuilder.AlterColumn<int>(
                name: "MusicCatalogId",
                table: "ShopCart",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AudioCatalogId",
                table: "ShopCart",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopCart_AudioCatalog_AudioCatalogId",
                table: "ShopCart",
                column: "AudioCatalogId",
                principalTable: "AudioCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopCart_MusicCatalog_MusicCatalogId",
                table: "ShopCart",
                column: "MusicCatalogId",
                principalTable: "MusicCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
