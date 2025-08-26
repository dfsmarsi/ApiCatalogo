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
        private readonly IRepository<Produto> _repository;
        private readonly IProdutoRepository _produtoRepository;

        public ProdutosController(IRepository<Produto> repository, IProdutoRepository produtoRepository)
        {
            _repository = repository;
            _produtoRepository = produtoRepository;
        }

        [HttpGet("produtos/{id}")]
        public ActionResult <IEnumerable<Produto>> GetProdutosPorCategoria(int id)
        {
            var produtos = _produtoRepository.GetProdutosPorCategoria(id);

            if(produtos is null)
                return NotFound();

            return Ok(produtos);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> GetTodosProdutos()
        {
            var produtos = _repository.GetAll();

            if (produtos == null)
                return NotFound();

            return Ok(produtos);
        }

        // (From Services) Model binding de services por DI e inferencia, sem atributo e sem injeção no construtor
        [HttpGet("GetService/{nome:alpha}")]
        public ActionResult<string> GetSaudacaoService(IMeuServico meuServico, string nome) {
            return meuServico.Saudacao(nome);
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public ActionResult<Produto> GetProdutoPorId(int id)
        {
            var produto = _repository.GetId(p=> p.IdProduto == id);

            if (produto == null)
                return NotFound();

            return Ok(produto);
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (produto is null)
                return BadRequest();

            var novoProduto = _repository.Create(produto);

            return new CreatedAtRouteResult("ObterProduto",
                new { id = novoProduto.IdProduto }, novoProduto);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if (id != produto.IdProduto)
                return BadRequest();

            var produtoAlterado = _repository.Update(produto);

            if (produtoAlterado is null)
                return StatusCode(500, $"Falha ao atualizar o produto código: {produto.IdProduto}");

            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _repository.GetId(p => p.IdProduto == id);

            if (produto is null)
                return StatusCode(500, $"Produto {id} não encontrado.");

            var produtoDeletado = _repository.Delete(produto);         
            
            return Ok(produtoDeletado);
        }

    }
}
