using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackBookRentals.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Libro");

            migrationBuilder.EnsureSchema(
                name: "Cliente");

            migrationBuilder.EnsureSchema(
                name: "Pedido");

            migrationBuilder.EnsureSchema(
                name: "Pedidos_Libros");

            migrationBuilder.EnsureSchema(
                name: "Usuario");

            migrationBuilder.CreateTable(
                name: "Book",
                schema: "Libro",
                columns: table => new
                {
                    id_libro = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    autor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    isbn = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Book", x => x.id_libro);
                });

            migrationBuilder.CreateTable(
                name: "Client",
                schema: "Cliente",
                columns: table => new
                {
                    id_cliente = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    nombres = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    apellidos = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    dni = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    edad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.id_cliente);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "Usuario",
                columns: table => new
                {
                    id_usuario = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    user_name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    user_pass = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.id_usuario);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                schema: "Pedido",
                columns: table => new
                {
                    id_pedido = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    fecha_pedido = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    estado = table.Column<bool>(type: "bit", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.id_pedido);
                    table.ForeignKey(
                        name: "FK_Pedido_Cliente",
                        column: x => x.ClientId,
                        principalSchema: "Cliente",
                        principalTable: "Client",
                        principalColumn: "id_cliente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pedido_Libro",
                schema: "Pedidos_Libros",
                columns: table => new
                {
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedido_Libro", x => new { x.OrderId, x.BookId });
                    table.ForeignKey(
                        name: "FK_Pedido_Libro_Libro",
                        column: x => x.BookId,
                        principalSchema: "Libro",
                        principalTable: "Book",
                        principalColumn: "id_libro",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pedido_Libro_Pedido",
                        column: x => x.OrderId,
                        principalSchema: "Pedido",
                        principalTable: "Order",
                        principalColumn: "id_pedido",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Book_isbn",
                schema: "Libro",
                table: "Book",
                column: "isbn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Client_dni",
                schema: "Cliente",
                table: "Client",
                column: "dni",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_ClientId",
                schema: "Pedido",
                table: "Order",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedido_Libro_BookId",
                schema: "Pedidos_Libros",
                table: "Pedido_Libro",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_User_user_name",
                schema: "Usuario",
                table: "User",
                column: "user_name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pedido_Libro",
                schema: "Pedidos_Libros");

            migrationBuilder.DropTable(
                name: "User",
                schema: "Usuario");

            migrationBuilder.DropTable(
                name: "Book",
                schema: "Libro");

            migrationBuilder.DropTable(
                name: "Order",
                schema: "Pedido");

            migrationBuilder.DropTable(
                name: "Client",
                schema: "Cliente");
        }
    }
}
