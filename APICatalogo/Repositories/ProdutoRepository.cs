using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using System.Threading.Tasks;
using X.PagedList;

namespace APICatalogo.Repositories;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    public ProdutoRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IPagedList<Produto>> GetProdutosFiltroPorPrecoAsync(ProdutosFiltroPreco produtoFiltroParams)
    {
        var produtos = await GetAllAsync();

        if(produtoFiltroParams.Preco.HasValue && !string.IsNullOrEmpty(produtoFiltroParams.PrecoCriterio))
        {
            if(produtoFiltroParams.PrecoCriterio.Equals("maior", StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos.Where(p => p.Preco >= produtoFiltroParams.Preco.Value).OrderBy(p => p.Preco);
            }
            else if(produtoFiltroParams.PrecoCriterio.Equals("menor", StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos.Where(p => p.Preco <= produtoFiltroParams.Preco.Value).OrderBy(p => p.Preco);
            }
            else if(produtoFiltroParams.PrecoCriterio.Equals("igual", StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos.Where(p => p.Preco == produtoFiltroParams.Preco.Value).OrderBy(p => p.Preco);
            }
        }

        var produtosFiltrados = await produtos.ToPagedListAsync(produtoFiltroParams.PageNumber, produtoFiltroParams.PageSize);

        return produtosFiltrados;
    }

    public async Task<IPagedList<Produto>> GetProdutosPaginadosAsync(ProdutoParameters produtoParams)
    {
        var produtos = await GetAllAsync();
        var produtosOrdenados = produtos.OrderBy(p => p.IdProduto).AsQueryable();
        var produtosOrdenadosPaginados = await produtosOrdenados.ToPagedListAsync(produtoParams.PageNumber, produtoParams.PageSize);

        return produtosOrdenadosPaginados;
    }

    //public IEnumerable<Produto> GetProdutosPaginados(ProdutoParameters produtoParams)
    //{
    //    return GetAll()
    //        .OrderBy(p => p.IdProduto)
    //        .Skip((produtoParams.PageNumber - 1) * produtoParams.PageSize)
    //        .Take(produtoParams.PageSize);
    //}

    public async Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id)
    {
        var produtos = await GetAllAsync();
        return produtos.Where(c => c.IdCategoria == id);
    }
}
