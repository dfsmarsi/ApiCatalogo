using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace APICatalogo.Migrations
{
    /// <inheritdoc />
    public partial class MigracaoInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categorias",
                columns: table => new
                {
                    idcategoria = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    urlimagem = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categorias", x => x.idcategoria);
                });

            migrationBuilder.CreateTable(
                name: "produtos",
                columns: table => new
                {
                    idproduto = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    descricao = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    preco = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    urlimagem = table.Column<string>(type: "text", nullable: true),
                    qtdeestoque = table.Column<float>(type: "real", nullable: false),
                    datacadastro = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    idcategoria = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_produtos", x => x.idproduto);
                    table.ForeignKey(
                        name: "FK_produtos_categorias_idcategoria",
                        column: x => x.idcategoria,
                        principalTable: "categorias",
                        principalColumn: "idcategoria",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_produtos_idcategoria",
                table: "produtos",
                column: "idcategoria");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "produtos");

            migrationBuilder.DropTable(
                name: "categorias");
        }
    }
}
