using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AllSopFoodService.Migrations
{
    public partial class ManyCartSchemaAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CouponCodes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "CouponCodes",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "CouponCodes",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "ShoppingCartItems");

            migrationBuilder.DropColumn(
                name: "IsInCart",
                table: "FoodProducts");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "ShoppingCarts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "ShoppingCartItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)")
                .Annotation("SqlServer:Identity", "1, 1");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "UserName",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "CartID",
                table: "ShoppingCartItems");

            migrationBuilder.DropColumn(
                name: "CartItemId",
                table: "FoodProducts");

            migrationBuilder.AlterColumn<string>(
                name: "ItemId",
                table: "ShoppingCartItems",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "ShoppingCartItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsInCart",
                table: "FoodProducts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "CouponCodes",
                columns: new[] { "Id", "CouponCode", "IsClaimed" },
                values: new object[] { 10, "10OFFPROMODRI", false });

            migrationBuilder.InsertData(
                table: "CouponCodes",
                columns: new[] { "Id", "CouponCode", "IsClaimed" },
                values: new object[] { 4, "5OFFPROMOALL", false });

            migrationBuilder.InsertData(
                table: "CouponCodes",
                columns: new[] { "Id", "CouponCode", "IsClaimed" },
                values: new object[] { 36, "20OFFPROMOALL", false });
        }
    }
}
