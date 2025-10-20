using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        Task<PagedList<Categoria>> GetCategoriasPaginadasAsync(CategoriaParameters categoriaParams);

        Task<PagedList<Categoria>> GetCategoriasFiltroPorNomeAsync(CategoriaFiltroNome categoriaFiltroParams);
    }
}
