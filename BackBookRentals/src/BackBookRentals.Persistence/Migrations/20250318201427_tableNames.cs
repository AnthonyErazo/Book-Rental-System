using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackBookRentals.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class tableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Pedido_Libro",
                schema: "Pedidos_Libros",
                table: "Pedido_Libro");

            migrationBuilder.RenameTable(
                name: "Pedido_Libro",
                schema: "Pedidos_Libros",
                newName: "OrderBook",
                newSchema: "Pedidos_Libros");

            migrationBuilder.RenameIndex(
                name: "IX_Pedido_Libro_BookId",
                schema: "Pedidos_Libros",
                table: "OrderBook",
                newName: "IX_OrderBook_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderBook",
                schema: "Pedidos_Libros",
                table: "OrderBook",
                columns: new[] { "Id", "BookId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderBook",
                schema: "Pedidos_Libros",
                table: "OrderBook");

            migrationBuilder.RenameTable(
                name: "OrderBook",
                schema: "Pedidos_Libros",
                newName: "Pedido_Libro",
                newSchema: "Pedidos_Libros");

            migrationBuilder.RenameIndex(
                name: "IX_OrderBook_BookId",
                schema: "Pedidos_Libros",
                table: "Pedido_Libro",
                newName: "IX_Pedido_Libro_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pedido_Libro",
                schema: "Pedidos_Libros",
                table: "Pedido_Libro",
                columns: new[] { "Id", "BookId" });
        }
    }
}
