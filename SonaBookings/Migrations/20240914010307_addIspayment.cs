using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SonaBookings.Migrations
{
    /// <inheritdoc />
    public partial class addIspayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "InvoiceAmount",
                table: "Invoices",
                type: "decimal(8,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,0)");

            migrationBuilder.AddColumn<bool>(
                name: "IsPayment",
                table: "Bookings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPayment",
                table: "Bookings");

            migrationBuilder.AlterColumn<decimal>(
                name: "InvoiceAmount",
                table: "Invoices",
                type: "decimal(8,0)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,0)",
                oldNullable: true);
        }
    }
}
