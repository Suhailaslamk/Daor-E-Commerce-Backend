using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Daor_E_Commerce.Migrations
{
    /// <inheritdoc />
    public partial class AddNormalizedNameToCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ShippingAddresses_ShippingAddressId1",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ShippingAddressId1",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingAddressId1",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "Categories");

            migrationBuilder.AddColumn<int>(
                name: "ShippingAddressId1",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShippingAddressId1",
                table: "Orders",
                column: "ShippingAddressId1",
                unique: true,
                filter: "[ShippingAddressId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ShippingAddresses_ShippingAddressId1",
                table: "Orders",
                column: "ShippingAddressId1",
                principalTable: "ShippingAddresses",
                principalColumn: "Id");
        }
    }
}
