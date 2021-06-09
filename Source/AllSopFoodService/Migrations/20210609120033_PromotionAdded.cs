using Microsoft.EntityFrameworkCore.Migrations;

namespace AllSopFoodService.Migrations
{
    public partial class PromotionAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CouponCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CouponCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsClaimed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CouponCodes", x => x.Id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CouponCodes");
        }
    }
}
