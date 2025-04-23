using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo.Migrations
{
    public partial class InserirDadosEmCategoriasEProdutos : Migration
    {
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("INSERT INTO categorias (nome, urlimagem) " +
                   "SELECT 'Bebidas', 'bebida.jpg' " +
                   "WHERE NOT EXISTS (SELECT 1 FROM categorias WHERE nome = 'Bebidas');");

            mb.Sql("INSERT INTO categorias (nome, urlimagem) " +
                   "SELECT 'Lanches', 'lanche.jpg' " +
                   "WHERE NOT EXISTS (SELECT 1 FROM categorias WHERE nome = 'Lanches');");

            mb.Sql("INSERT INTO categorias (nome, urlimagem) " +
                   "SELECT 'Sobremesas', 'doce.jpg' " +
                   "WHERE NOT EXISTS (SELECT 1 FROM categorias WHERE nome = 'Sobremesas');");


            mb.Sql("INSERT INTO produtos (nome, descricao, preco, urlimagem, qtdeestoque, datacadastro, idcategoria) " +
                   "SELECT 'Coca-Cola Zero', 'Refrigerante de cola sem açúcar', 5.45, 'bebida.jpg', 100, now(), idcategoria " +
                   "FROM categorias WHERE nome = 'Bebidas' LIMIT 1;");

            mb.Sql("INSERT INTO produtos (nome, descricao, preco, urlimagem, qtdeestoque, datacadastro, idcategoria) " +
                   "SELECT 'X-Tudão do Braia', 'Duplo hambúrguer, ovos, bacon', 32.99, 'lanche.jpg', 100, now(), idcategoria " +
                   "FROM categorias WHERE nome = 'Lanches' LIMIT 1;");

            mb.Sql("INSERT INTO produtos (nome, descricao, preco, urlimagem, qtdeestoque, datacadastro, idcategoria) " +
                   "SELECT 'Pudim ne pae', 'Pudim que o Bills queria', 4.99, 'pudim.jpg', 100, now(), idcategoria " +
                   "FROM categorias WHERE nome = 'Sobremesas' LIMIT 1;");
        }

        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("DELETE FROM produtos WHERE nome IN ('Coca-Cola Zero', 'X-Tudão do Braia', 'Pudim ne pae');");
            mb.Sql("DELETE FROM categorias WHERE nome IN ('Bebidas', 'Lanches', 'Sobremesas');");
        }
    }
}
