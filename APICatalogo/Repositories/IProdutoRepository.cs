using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        PagedList<Produto> GetProdutosPaginados(ProdutoParameters produtoParams);
        IEnumerable<Produto> GetProdutosPorCategoria(int id);
    }
}
