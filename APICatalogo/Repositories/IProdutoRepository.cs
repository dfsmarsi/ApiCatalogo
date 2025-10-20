using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task <PagedList<Produto>> GetProdutosPaginadosAsync(ProdutoParameters produtoParams);
        Task <PagedList<Produto>> GetProdutosFiltroPorPrecoAsync(ProdutosFiltroPreco produtoFiltroParams);
        Task <IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id);
    }
}
