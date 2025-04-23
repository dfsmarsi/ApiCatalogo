using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
                _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> GetTodosProdutos()
        {
            var produtos = _context.Produtos.ToList();
            if (produtos is null)
                return NotFound("Sem produtos para retornar");
            return produtos;
        }

        [HttpGet("{id:int}")]
        public ActionResult<Produto> GetProdutoPorId(int id) 
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.IdProduto == id);
            if (produto is null)
                return NotFound($"Produto codigo {id} não encontrado!");
            return produto;
        }

    }
}
