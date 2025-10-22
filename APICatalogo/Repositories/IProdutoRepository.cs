using APICatalogo.Models;
using APICatalogo.Pagination;
using X.PagedList;

namespace APICatalogo.Repositories
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task <IPagedList<Produto>> GetProdutosPaginadosAsync(ProdutoParameters produtoParams);
        Task <IPagedList<Produto>> GetProdutosFiltroPorPrecoAsync(ProdutosFiltroPreco produtoFiltroParams);
        Task <IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id);
    }
}
