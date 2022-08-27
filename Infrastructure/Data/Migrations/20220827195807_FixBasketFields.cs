using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class FixBasketFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketProducts_Promotions_PromotionId",
                table: "BasketProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderProducts_Promotions_PromotionId",
                table: "OrderProducts");

            migrationBuilder.DropIndex(
                name: "IX_OrderProducts_PromotionId",
                table: "OrderProducts");

            migrationBuilder.DropIndex(
                name: "IX_BasketProducts_PromotionId",
                table: "BasketProducts");

            migrationBuilder.DropColumn(
                name: "PromotionId",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "PromotionId",
                table: "BasketProducts");

            migrationBuilder.AddColumn<double>(
                name: "Total",
                table: "Orders",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "OrderProducts",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Total",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "OrderProducts");

            migrationBuilder.AddColumn<int>(
                name: "PromotionId",
                table: "OrderProducts",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PromotionId",
                table: "BasketProducts",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_PromotionId",
                table: "OrderProducts",
                column: "PromotionId");

            migrationBuilder.CreateIndex(
                name: "IX_BasketProducts_PromotionId",
                table: "BasketProducts",
                column: "PromotionId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketProducts_Promotions_PromotionId",
                table: "BasketProducts",
                column: "PromotionId",
                principalTable: "Promotions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProducts_Promotions_PromotionId",
                table: "OrderProducts",
                column: "PromotionId",
                principalTable: "Promotions",
                principalColumn: "Id");
        }
    }
}
