using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackBookRentals.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class users : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderId",
                schema: "Pedidos_Libros",
                table: "Pedido_Libro",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "Pedidos_Libros",
                table: "Pedido_Libro",
                newName: "OrderId");
        }
    }
}
