using Microsoft.EntityFrameworkCore.Migrations;

namespace AllSopFoodService.Migrations
{
    public partial class removeCartItemEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodProducts_ShoppingCartItems_CartItemId",
                table: "FoodProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartItems_ShoppingCarts_CartID",
                table: "ShoppingCartItems");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCartItems_CartID",
                table: "ShoppingCartItems");

            migrationBuilder.DropIndex(
                name: "IX_FoodProducts_CartItemId",
                table: "FoodProducts");

            migrationBuilder.DeleteData(
                table: "CouponCodes",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "CouponCodes",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "CouponCodes",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DropColumn(
                name: "CartID",
                table: "ShoppingCartItems");

            migrationBuilder.DropColumn(
                name: "CartItemId",
                table: "FoodProducts");

            migrationBuilder.AddColumn<int>(
                name: "QuantityInCart",
                table: "FoodProducts_Carts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "CouponCodes",
                columns: new[] { "Id", "CouponCode", "IsClaimed" },
                values: new object[] { 31, "10OFFPROMODRI", false });

            migrationBuilder.InsertData(
                table: "CouponCodes",
                columns: new[] { "Id", "CouponCode", "IsClaimed" },
                values: new object[] { 46, "5OFFPROMOALL", false });

            migrationBuilder.InsertData(
                table: "CouponCodes",
                columns: new[] { "Id", "CouponCode", "IsClaimed" },
                values: new object[] { 20, "20OFFPROMOALL", false });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CouponCodes",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "CouponCodes",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "CouponCodes",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DropColumn(
                name: "QuantityInCart",
                table: "FoodProducts_Carts");

            migrationBuilder.AddColumn<int>(
                name: "CartID",
                table: "ShoppingCartItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CartItemId",
                table: "FoodProducts",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "CouponCodes",
                columns: new[] { "Id", "CouponCode", "IsClaimed" },
                values: new object[] { 9, "10OFFPROMODRI", false });

            migrationBuilder.InsertData(
                table: "CouponCodes",
                columns: new[] { "Id", "CouponCode", "IsClaimed" },
                values: new object[] { 29, "5OFFPROMOALL", false });

            migrationBuilder.InsertData(
                table: "CouponCodes",
                columns: new[] { "Id", "CouponCode", "IsClaimed" },
                values: new object[] { 45, "20OFFPROMOALL", false });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartItems_CartID",
                table: "ShoppingCartItems",
                column: "CartID");

            migrationBuilder.CreateIndex(
                name: "IX_FoodProducts_CartItemId",
                table: "FoodProducts",
                column: "CartItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodProducts_ShoppingCartItems_CartItemId",
                table: "FoodProducts",
                column: "CartItemId",
                principalTable: "ShoppingCartItems",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartItems_ShoppingCarts_CartID",
                table: "ShoppingCartItems",
                column: "CartID",
                principalTable: "ShoppingCarts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
