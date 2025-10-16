using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        PagedList<Produto> GetProdutosPaginados(ProdutoParameters produtoParams);
        PagedList<Produto> GetProdutosFiltroPorPreco(ProdutosFiltroPreco produtoFiltroParams);
        IEnumerable<Produto> GetProdutosPorCategoria(int id);
    }
}
