using Microsoft.EntityFrameworkCore.Migrations;
using OrderEase.Data.Enums;

#nullable disable

namespace OrderEase.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Seed Orders Table
            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderID", "ItemName", "Quantity", "TotalPrice",
         "OrderDate", "DeliveryDate", "Supplier", "OrderStatus"},
                values: new object[,]
                {
         {1, 2, 15.00m, DateTime.Now.AddDays(-10),  DateTime.Now.AddDays(5),
         "King Fresh", (int)OrderStatus.Shipped},
         {2, 6, 5.00m, DateTime.Now.AddDays(-12),  DateTime.Now.AddDays(8),
         "Greenfield",(int)OrderStatus.Cancelled},
          {3, 7, 10.00m, DateTime.Now.AddDays(-6),  DateTime.Now.AddDays(4),
         "Orange Groove",(int)OrderStatus.Pending}
                });

            //Seed Items table
            migrationBuilder.InsertData(
               table: "Items",
               columns: new[] { "ItemID", "ItemName", "Price",
        "QuantityInStock", "OrderID"},
               values: new object[,]
               {
        {1, "Selati Sugar", 25.00m, 18, 1},
        {2, "Red Apples", 12.00m, 10, 3},
        {4, "peanuts", 20.00m, 15, 2}
               });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
