using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Repositories;
using APICatalogo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;

        public ProdutosController(IUnitOfWork uof)
        {
            _uof = uof;
        }

        [HttpGet("produtosporcategoria/{id}")]
        public ActionResult <IEnumerable<Produto>> GetProdutosPorCategoria(int id)
        {
            var produtos = _uof.ProdutoRepository.GetProdutosPorCategoria(id);

            if(produtos is null)
                return NotFound();

            return Ok(produtos);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> GetTodosProdutos()
        {
            var produtos = _uof.ProdutoRepository.GetAll();

            if (produtos == null)
                return NotFound();

            return Ok(produtos);
        }

        // (From Services) Model binding de services por DI e inferencia, sem atributo e sem injeção no construtor
        //[HttpGet("GetService/{nome:alpha}")]
        //public ActionResult<string> GetSaudacaoService(IMeuServico meuServico, string nome) {
        //    return meuServico.Saudacao(nome);
        //}

        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public ActionResult<Produto> GetProdutoPorId(int id)
        {
            var produto = _uof.ProdutoRepository.GetId(p=> p.IdProduto == id);

            if (produto == null)
                return NotFound();

            return Ok(produto);
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (produto is null)
                return BadRequest();

            var novoProduto = _uof.ProdutoRepository.Create(produto);
            _uof.Commit();

            return new CreatedAtRouteResult("ObterProduto",
                new { id = novoProduto.IdProduto }, novoProduto);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if (id != produto.IdProduto)
                return BadRequest();

            var produtoAlterado = _uof.ProdutoRepository.Update(produto);
            _uof.Commit();

            if (produtoAlterado is null)
                return StatusCode(500, $"Falha ao atualizar o produto código: {produto.IdProduto}");

            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _uof.ProdutoRepository.GetId(p => p.IdProduto == id);

            if (produto is null)
                return StatusCode(500, $"Produto {id} não encontrado.");

            var produtoDeletado = _uof.ProdutoRepository.Delete(produto);  
            _uof.Commit();

            return Ok(produtoDeletado);
        }

    }
}
