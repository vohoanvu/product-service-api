using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AllSopFoodService.Migrations
{
    public partial class CartItemCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShoppingCartItems",
                columns: table => new
                {
                    ItemId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true,
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCartItems", x => x.ItemId);
                    table.ForeignKey(
                        name: "FK_ShoppingCartItems_FoodProducts_ProductId",
                        column: x => x.ProductId,
                        principalTable: "FoodProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartItems_ProductId",
                table: "ShoppingCartItems",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShoppingCartItems");
        }
    }
}
