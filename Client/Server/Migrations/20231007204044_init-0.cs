using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Client.Server.Migrations
{
    /// <inheritdoc />
    public partial class init0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrdersId",
                table: "MusicCatalog",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrdersId",
                table: "AudioCatalog",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MusicCatalog_OrdersId",
                table: "MusicCatalog",
                column: "OrdersId");

            migrationBuilder.CreateIndex(
                name: "IX_AudioCatalog_OrdersId",
                table: "AudioCatalog",
                column: "OrdersId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ApplicationUserId",
                table: "Orders",
                column: "ApplicationUserId",
                unique: true,
                filter: "[ApplicationUserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AudioCatalog_Orders_OrdersId",
                table: "AudioCatalog",
                column: "OrdersId",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MusicCatalog_Orders_OrdersId",
                table: "MusicCatalog",
                column: "OrdersId",
                principalTable: "Orders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AudioCatalog_Orders_OrdersId",
                table: "AudioCatalog");

            migrationBuilder.DropForeignKey(
                name: "FK_MusicCatalog_Orders_OrdersId",
                table: "MusicCatalog");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_MusicCatalog_OrdersId",
                table: "MusicCatalog");

            migrationBuilder.DropIndex(
                name: "IX_AudioCatalog_OrdersId",
                table: "AudioCatalog");

            migrationBuilder.DropColumn(
                name: "OrdersId",
                table: "MusicCatalog");

            migrationBuilder.DropColumn(
                name: "OrdersId",
                table: "AudioCatalog");
        }
    }
}
