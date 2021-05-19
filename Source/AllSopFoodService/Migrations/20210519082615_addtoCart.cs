namespace AllSopFoodService.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class addtoCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.AddColumn<bool>(
                    name: "IsInCart",
                    table: "FoodProducts",
                    nullable: false,
                    defaultValue: false
                );

        protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropColumn(name: "IsInCart", table: "FoodProducts");
    }
}
