using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITS.PreEsame.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDeliveryCodeToOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeliveryCode",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryCode",
                table: "Orders");
        }
    }
}
