using APICatalogo.Context;
using APICatalogo.DTO;
using APICatalogo.Models;
using APICatalogo.Repositories;
using APICatalogo.Services;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;

        public ProdutosController(IUnitOfWork uof, IMapper mapper)
        {
            _uof = uof;
            _mapper = mapper;
        }

        [HttpGet("produtosporcategoria/{id}")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosPorCategoria(int id)
        {
            var produtos = _uof.ProdutoRepository.GetProdutosPorCategoria(id);

            if (produtos is null)
                return NotFound();

            var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

            return Ok(produtosDTO);
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProdutoDTO>> GetTodosProdutos()
        {
            var produtos = _uof.ProdutoRepository.GetAll();

            if (produtos == null)
                return NotFound();

            var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

            return Ok(produtosDTO);
        }

        // (From Services) Model binding de services por DI e inferencia, sem atributo e sem injeção no construtor
        //[HttpGet("GetService/{nome:alpha}")]
        //public ActionResult<string> GetSaudacaoService(IMeuServico meuServico, string nome) {
        //    return meuServico.Saudacao(nome);
        //}

        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public ActionResult<ProdutoDTO> GetProdutoPorId(int id)
        {
            var produto = _uof.ProdutoRepository.GetId(p => p.IdProduto == id);

            if (produto == null)
                return NotFound();

            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

            return Ok(produtoDTO);
        }

        [HttpPost]
        public ActionResult<ProdutoDTO> Post(ProdutoDTO produtoDTO)
        {
            if (produtoDTO is null)
                return BadRequest();

            var produto = _mapper.Map<Produto>(produtoDTO);

            var novoProduto = _uof.ProdutoRepository.Create(produto);
            _uof.Commit();

            var novoProdutoDTO = _mapper.Map<ProdutoDTO>(novoProduto);

            return new CreatedAtRouteResult("ObterProduto",
                new { id = novoProdutoDTO.IdProduto }, novoProdutoDTO);
        }

        [HttpPatch("{id}/UpdatePartial")]
        public ActionResult<ProdutoDTOUpdateResponse> Patch(int id, JsonPatchDocument<ProdutoDTOUpdateRequest> patchProdutoDTO)
        {
            if (patchProdutoDTO is null || id < 0)
                return BadRequest();

            var produto = _uof.ProdutoRepository.GetId(p => p.IdProduto == id);

            if(produto is null)
                return NotFound($"Produto {id} não encontrado");

            var produtoUpdateRequest = _mapper.Map<ProdutoDTOUpdateRequest>(produto);
            
            patchProdutoDTO.ApplyTo(produtoUpdateRequest, ModelState);
            if(!ModelState.IsValid || !TryValidateModel(produtoUpdateRequest))
                return BadRequest(ModelState);

            _mapper.Map(produtoUpdateRequest, produto);

            _uof.ProdutoRepository.Update(produto);
            _uof.Commit();

            return Ok(_mapper.Map<ProdutoDTOUpdateResponse>(produto));
        }

        [HttpPut("{id:int}")]
        public ActionResult<ProdutoDTO> Put(int id, ProdutoDTO produtoDTO)
        {
            if (id != produtoDTO.IdProduto)
                return BadRequest();

            var produto = _mapper.Map<Produto>(produtoDTO);

            var produtoAlterado = _uof.ProdutoRepository.Update(produto);
            _uof.Commit();

            if (produtoAlterado is null)
                return StatusCode(500, $"Falha ao atualizar o produto código: {produto.IdProduto}");

            var produtoAlteradoDTO = _mapper.Map<ProdutoDTO>(produtoAlterado);

            return Ok(produtoAlteradoDTO);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<ProdutoDTO> Delete(int id)
        {
            var produto = _uof.ProdutoRepository.GetId(p => p.IdProduto == id);

            if (produto is null)
                return StatusCode(500, $"Produto {id} não encontrado.");

            var produtoDeletado = _uof.ProdutoRepository.Delete(produto);
            _uof.Commit();

            var produtoDeletadoDTO = _mapper.Map<ProdutoDTO>(produtoDeletado);

            return Ok(produtoDeletadoDTO);
        }

    }
}
