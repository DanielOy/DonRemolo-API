using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class ProductRelationNumberAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductRelationNumber",
                table: "BasketProducts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderPromotions_OrderId",
                table: "OrderPromotions",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderPromotions_Orders_OrderId",
                table: "OrderPromotions",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderPromotions_Orders_OrderId",
                table: "OrderPromotions");

            migrationBuilder.DropIndex(
                name: "IX_OrderPromotions_OrderId",
                table: "OrderPromotions");

            migrationBuilder.DropColumn(
                name: "ProductRelationNumber",
                table: "BasketProducts");
        }
    }
}
