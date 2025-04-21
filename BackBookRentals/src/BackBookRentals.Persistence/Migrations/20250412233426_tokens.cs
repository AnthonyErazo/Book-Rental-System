using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackBookRentals.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class tokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "Usuario_Token",
                table: "UserToken");

            migrationBuilder.DropColumn(
                name: "is_valid",
                schema: "Usuario_Token",
                table: "UserToken");

            migrationBuilder.DropColumn(
                name: "enabled",
                schema: "Usuario",
                table: "User");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "Usuario_Token",
                table: "UserToken",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<bool>(
                name: "is_valid",
                schema: "Usuario_Token",
                table: "UserToken",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "enabled",
                schema: "Usuario",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }
    }
}
