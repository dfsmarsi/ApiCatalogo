using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo.Migrations
{
    /// <inheritdoc />
    public partial class AjusteDataCadastro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "datacadastro",
                table: "produtos",
                type: "timestamptz",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "datacadastro",
                table: "produtos",
                type: "TIMESTAMP",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamptz");
        }
    }
}
