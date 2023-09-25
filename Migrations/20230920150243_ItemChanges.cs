using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderEase.Migrations
{
    /// <inheritdoc />
    public partial class ItemChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelectedOrderID",
                table: "Items");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SelectedOrderID",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
