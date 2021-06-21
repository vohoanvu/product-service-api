using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AllSopFoodService.Migrations
{
    public partial class LogTableAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CouponCodes",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "CouponCodes",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MessageTemplate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Level = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Exception = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Properties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogEvent = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CouponCodes",
                columns: new[] { "Id", "CouponCode", "IsClaimed" },
                values: new object[] { 4, "5OFFPROMOALL", false });

            migrationBuilder.InsertData(
                table: "CouponCodes",
                columns: new[] { "Id", "CouponCode", "IsClaimed" },
                values: new object[] { 36, "20OFFPROMOALL", false });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DeleteData(
                table: "CouponCodes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "CouponCodes",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.InsertData(
                table: "CouponCodes",
                columns: new[] { "Id", "CouponCode", "IsClaimed" },
                values: new object[] { 39, "5OFFPROMOALL", false });

            migrationBuilder.InsertData(
                table: "CouponCodes",
                columns: new[] { "Id", "CouponCode", "IsClaimed" },
                values: new object[] { 37, "20OFFPROMOALL", false });
        }
    }
}
