using APICatalogo.Context;
using APICatalogo.Models;

namespace APICatalogo.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly AppDbContext _context;

        public IQueryable<Produto> Get()
        {
            return _context.Produtos;
        }

        public Produto GetId(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.IdProduto == id);

            if (produto == null)
                throw new InvalidOperationException("Produto não encontrado!");

            return produto;
        }
        public Produto Create(Produto produto)
        {
            if (produto == null)
                throw new InvalidOperationException("Produto não informado!");

            _context.Produtos.Add(produto);
            _context.SaveChanges();

            return produto;
        }

        public bool Update(Produto produto)
        {
            if (produto is null) 
                throw new InvalidOperationException("Produto não informado!");

            if (_context.Produtos.Any(p=> p.IdProduto == produto.IdProduto))
            {
                _context.Produtos.Update(produto);
                _context.SaveChanges();
                return true;
            }

            return false;
        }
        
        public bool Delete(int id)
        {
            var produto = _context.Produtos.Find(id);

            if (produto is not null)
            {
                _context.Produtos.Remove(produto);
                _context.SaveChanges();
                return true;
            }
              
            return false;
        }
    }
}
