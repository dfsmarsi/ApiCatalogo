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
        private readonly IProdutoRepository _repository;

        public ProdutosController(IProdutoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> GetTodosProdutos()
        {
            var produtos = _repository.Get().ToList();

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
            var produto = _repository.GetId(id);

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

            bool produtoAlterado = _repository.Update(produto);

            if (!produtoAlterado)
                return StatusCode(500, $"Falha ao atualizar o produto código: {produto.IdProduto}");

            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            bool produtoDeletado = _repository.Delete(id);

            if (!produtoDeletado)
                return StatusCode(500, $"Falha ao deletar produto código: {id}");

            return Ok($"Produto código {id}, deletado com sucesso!");
        }

    }
}
