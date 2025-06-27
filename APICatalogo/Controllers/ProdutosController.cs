using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

            try
            {
                var produtos = _context.Produtos.ToList();
                if (produtos is null)
                    return NotFound("Sem produtos para retornar");
                return produtos;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema na sua requisição!");
            }
        }

        // (From Services) Model binding de services por DI e inferencia, sem atributo e sem injeção no construtor
        [HttpGet("GetService/{nome:alpha}")]
        public ActionResult<string> GetSaudacaoService(IMeuServico meuServico, string nome) {
            return meuServico.Saudacao(nome);
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public async Task<ActionResult<Produto>> GetProdutoPorId(int id)
        {
            try
            {
                var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.IdProduto == id);
                if (produto is null)
                    return NotFound($"Produto codigo {id} não encontrado!");
                return produto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema na sua requisição!");
            }
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (produto is null)
                return BadRequest();

            _context.Produtos.Add(produto);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterProduto",
                new { id = produto.IdProduto }, produto);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if (id != produto.IdProduto)
                return BadRequest();

            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.IdProduto == id);

            if (produto is null)
                return NotFound("Produto não encontrado!");

            _context.Produtos.Remove(produto);
            _context.SaveChanges();

            return Ok(produto);
        }

    }
}
