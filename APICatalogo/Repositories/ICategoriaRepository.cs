using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        PagedList<Categoria> GetCategoriasPaginadas(CategoriaParameters categoriaParams);

        PagedList<Categoria> GetCategoriasFiltroPorNome(CategoriaFiltroNome categoriaFiltroParams);
    }
}
