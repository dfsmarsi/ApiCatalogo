using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using X.PagedList;

namespace APICatalogo.Repositories;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{

    public CategoriaRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IPagedList<Categoria>> GetCategoriasFiltroPorNomeAsync(CategoriaFiltroNome categoriaFiltroParams)
    {
        var categorias = await GetAllAsync();

        if(!string.IsNullOrEmpty(categoriaFiltroParams.Nome))
        {
            categorias = categorias.Where(c => c.Nome.Contains(categoriaFiltroParams.Nome, StringComparison.OrdinalIgnoreCase))
                .OrderBy(c => c.Nome);
        }

        //var categoriasFiltradas = PagedList<Categoria>.ToPagedList(categorias.AsQueryable(), categoriaFiltroParams.PageNumber, categoriaFiltroParams.PageSize);
        var categoriasFiltradas = await categorias.ToPagedListAsync(categoriaFiltroParams.PageNumber, categoriaFiltroParams.PageSize);
        
        return categoriasFiltradas;
    }

    public async Task<IPagedList<Categoria>> GetCategoriasPaginadasAsync(CategoriaParameters categoriaParams)
    {
        var categorias = await GetAllAsync();
        var categoriasOrdenadas = categorias.AsQueryable().OrderBy(c => c.Nome);
        //var categoriasOrdenasPaginadas = PagedList<Categoria>.ToPagedList(categoriasOrdenadas, categoriaParams.PageNumber, categoriaParams.PageSize);
        var categoriasOrdenasPaginadas = await categoriasOrdenadas.ToPagedListAsync(categoriaParams.PageNumber, categoriaParams.PageSize);

        return categoriasOrdenasPaginadas;
    }
}
