namespace AllSopFoodService.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class ManyToManyRelationWithShoppingCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CouponCodes",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "CouponCodes",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "CouponCodes",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.CreateTable(
                name: "ShoppingCarts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CartLabel = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCarts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FoodProducts_Carts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FoodProductId = table.Column<int>(type: "int", nullable: false),
                    ShoppingCartId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodProducts_Carts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoodProducts_Carts_FoodProducts_FoodProductId",
                        column: x => x.FoodProductId,
                        principalTable: "FoodProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoodProducts_Carts_ShoppingCarts_ShoppingCartId",
                        column: x => x.ShoppingCartId,
                        principalTable: "ShoppingCarts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CouponCodes",
                columns: new[] { "Id", "CouponCode", "IsClaimed" },
                values: new object[] { 10, "10OFFPROMODRI", false });

            migrationBuilder.InsertData(
                table: "CouponCodes",
                columns: new[] { "Id", "CouponCode", "IsClaimed" },
                values: new object[] { 39, "5OFFPROMOALL", false });

            migrationBuilder.InsertData(
                table: "CouponCodes",
                columns: new[] { "Id", "CouponCode", "IsClaimed" },
                values: new object[] { 37, "20OFFPROMOALL", false });

            migrationBuilder.CreateIndex(
                name: "IX_FoodProducts_Carts_FoodProductId",
                table: "FoodProducts_Carts",
                column: "FoodProductId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodProducts_Carts_ShoppingCartId",
                table: "FoodProducts_Carts",
                column: "ShoppingCartId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoodProducts_Carts");

            migrationBuilder.DropTable(
                name: "ShoppingCarts");

            migrationBuilder.DeleteData(
                table: "CouponCodes",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "CouponCodes",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "CouponCodes",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.InsertData(
                table: "CouponCodes",
                columns: new[] { "Id", "CouponCode", "IsClaimed" },
                values: new object[] { 36, "10OFFPROMODRI", false });

            migrationBuilder.InsertData(
                table: "CouponCodes",
                columns: new[] { "Id", "CouponCode", "IsClaimed" },
                values: new object[] { 43, "5OFFPROMOALL", false });

            migrationBuilder.InsertData(
                table: "CouponCodes",
                columns: new[] { "Id", "CouponCode", "IsClaimed" },
                values: new object[] { 11, "20OFFPROMOALL", false });
        }
    }
}
