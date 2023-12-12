using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Client.Server.Migrations
{
    /// <inheritdoc />
    public partial class v03 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShippingAddressId",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ShippingAddress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StreetAddres1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StreetAddress2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    IntNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingAddress", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_ShippingAddressId",
                table: "Order",
                column: "ShippingAddressId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_ShippingAddress_ShippingAddressId",
                table: "Order",
                column: "ShippingAddressId",
                principalTable: "ShippingAddress",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_ShippingAddress_ShippingAddressId",
                table: "Order");

            migrationBuilder.DropTable(
                name: "ShippingAddress");

            migrationBuilder.DropIndex(
                name: "IX_Order_ShippingAddressId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ShippingAddressId",
                table: "Order");
        }
    }
}
