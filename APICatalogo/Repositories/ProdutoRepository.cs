using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    public ProdutoRepository(AppDbContext context) : base(context)
    {
    }

    public PagedList<Produto> GetProdutosFiltroPorPreco(ProdutosFiltroPreco produtoFiltroParams)
    {
        var produtos = GetAll().AsQueryable();

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

        var produtosFiltrados = PagedList<Produto>.ToPagedList(produtos, produtoFiltroParams.PageNumber, produtoFiltroParams.PageSize);

        return produtosFiltrados;
    }

    public PagedList<Produto> GetProdutosPaginados(ProdutoParameters produtoParams)
    {
        var produtos = GetAll().OrderBy(p => p.IdProduto).AsQueryable();
        var produtosOrdenados = PagedList<Produto>.ToPagedList(produtos, produtoParams.PageNumber, produtoParams.PageSize);

        return produtosOrdenados;
    }

    //public IEnumerable<Produto> GetProdutosPaginados(ProdutoParameters produtoParams)
    //{
    //    return GetAll()
    //        .OrderBy(p => p.IdProduto)
    //        .Skip((produtoParams.PageNumber - 1) * produtoParams.PageSize)
    //        .Take(produtoParams.PageSize);
    //}

    public IEnumerable<Produto> GetProdutosPorCategoria(int id)
    {
        return GetAll().Where(c => c.IdCategoria == id);
    }
}
