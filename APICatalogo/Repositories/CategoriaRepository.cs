using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{

    public CategoriaRepository(AppDbContext context) : base(context)
    {
    }

    public PagedList<Categoria> GetCategoriasFiltroPorNome(CategoriaFiltroNome categoriaFiltroParams)
    {
        var categorias = GetAll().AsQueryable();

        if(!string.IsNullOrEmpty(categoriaFiltroParams.Nome))
        {
            categorias = categorias.Where(c => c.Nome.Contains(categoriaFiltroParams.Nome, StringComparison.OrdinalIgnoreCase))
                .OrderBy(c => c.Nome);
        }

        var categoriasFiltradas = PagedList<Categoria>.ToPagedList(categorias, categoriaFiltroParams.PageNumber, categoriaFiltroParams.PageSize);
        return categoriasFiltradas;
    }

    public PagedList<Categoria> GetCategoriasPaginadas(CategoriaParameters categoriaParams)
    {
        var categorias = GetAll().OrderBy(p => p.IdCategoria).AsQueryable();
        var categoriasOrdenadas = PagedList<Categoria>.ToPagedList(categorias, categoriaParams.PageNumber, categoriaParams.PageSize);

        return categoriasOrdenadas;
    }
}
