using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ristorante360.Migrations
{
    public partial class AdjustTotalAmountPrecision : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Total_Amount",
                table: "Order",
                type: "decimal(18, 2)", // Precisión de 18 dígitos totales, 2 decimales
                nullable: true,        // La columna sigue permitiendo valores nulos
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 0)", // Tipo anterior (sin decimales)
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Total_Amount",
                table: "Order",
                type: "decimal(18, 0)", // Tipo original (sin decimales)
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldNullable: true);
        }

    }
}
