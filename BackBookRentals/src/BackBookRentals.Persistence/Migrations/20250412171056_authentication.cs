using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackBookRentals.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class authentication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Usuario_Token");

            migrationBuilder.AlterColumn<string>(
                name: "user_pass",
                schema: "Usuario",
                table: "User",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "user_name",
                schema: "Usuario",
                table: "User",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<bool>(
                name: "enabled",
                schema: "Usuario",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.CreateTable(
                name: "UserToken",
                schema: "Usuario_Token",
                columns: table => new
                {
                    id_token = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    id_user = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    token_id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    is_valid = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToken", x => x.id_token);
                    table.ForeignKey(
                        name: "FK_UserToken_User_id_user",
                        column: x => x.id_user,
                        principalSchema: "Usuario",
                        principalTable: "User",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserToken_id_user",
                schema: "Usuario_Token",
                table: "UserToken",
                column: "id_user");

            migrationBuilder.CreateIndex(
                name: "IX_UserToken_token_id",
                schema: "Usuario_Token",
                table: "UserToken",
                column: "token_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserToken",
                schema: "Usuario_Token");

            migrationBuilder.DropColumn(
                name: "enabled",
                schema: "Usuario",
                table: "User");

            migrationBuilder.AlterColumn<string>(
                name: "user_pass",
                schema: "Usuario",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(60)",
                oldMaxLength: 60);

            migrationBuilder.AlterColumn<string>(
                name: "user_name",
                schema: "Usuario",
                table: "User",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }
    }
}
