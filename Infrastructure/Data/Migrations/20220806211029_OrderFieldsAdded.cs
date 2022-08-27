using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class OrderFieldsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: true),
                    AtHome = table.Column<bool>(type: "INTEGER", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    ContactName = table.Column<string>(type: "TEXT", nullable: true),
                    ContactNumber = table.Column<string>(type: "TEXT", nullable: true),
                    DelayDelivery = table.Column<bool>(type: "INTEGER", nullable: false),
                    DeliveryTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsCashPayment = table.Column<bool>(type: "INTEGER", nullable: false),
                    Cash = table.Column<double>(type: "REAL", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: true),
                    PromotionId = table.Column<int>(type: "INTEGER", nullable: true),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    DoughId = table.Column<int>(type: "INTEGER", nullable: true),
                    SizeId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderProducts_Doughs_DoughId",
                        column: x => x.DoughId,
                        principalTable: "Doughs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderProducts_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderProducts_Promotions_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "Promotions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderProducts_Sizes_SizeId",
                        column: x => x.SizeId,
                        principalTable: "Sizes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderIngredients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderId = table.Column<Guid>(type: "TEXT", nullable: false),
                    OrderProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    IngredientId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderIngredients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderIngredients_Ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "Ingredients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderIngredients_OrderProducts_OrderProductId",
                        column: x => x.OrderProductId,
                        principalTable: "OrderProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderIngredients_IngredientId",
                table: "OrderIngredients",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderIngredients_OrderProductId",
                table: "OrderIngredients",
                column: "OrderProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_DoughId",
                table: "OrderProducts",
                column: "DoughId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_OrderId",
                table: "OrderProducts",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_ProductId",
                table: "OrderProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_PromotionId",
                table: "OrderProducts",
                column: "PromotionId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_SizeId",
                table: "OrderProducts",
                column: "SizeId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderIngredients");

            migrationBuilder.DropTable(
                name: "OrderProducts");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
