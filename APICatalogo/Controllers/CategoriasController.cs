using APICatalogo.Context;
using APICatalogo.DTO;
using APICatalogo.DTO.Mappings;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly ILogger _logger;

        public CategoriasController(IUnitOfWork uof, ILogger logger)
        {
            _uof = uof;
            _logger = logger;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))] // Filtro personalisado para gerar logs
        public ActionResult<IEnumerable<CategoriaDTO>> BuscarTodasAsCategorias()
        {
            var categorias = _uof.CategoriaRepository.GetAll();

            if(categorias is null)
                return NotFound("Nenhuma categoria cadastrada!");

            var categoriasDTO = categorias.ToCategoriaDTOList();

            return Ok(categoriasDTO);
        }

        [HttpGet("{id:int}", Name = "BuscarCategoriaPorId")]
        public ActionResult<CategoriaDTO> BuscarCategoriaPorId(int id)
        {
            var categoria = _uof.CategoriaRepository.GetId(c=> c.IdCategoria == id);

            if (categoria is null)
            {
                _logger.LogWarning("----------------------- Get API/CategoriaId Categoria não encontrada  ----------------------");
                return NotFound($"Categoria código {id} não encontrada!");
            }

            var categoriaDTO = categoria.ToCategoriaDTO();

            return Ok(categoriaDTO);
        }

        [HttpPost]
        public ActionResult<CategoriaDTO> CriarCategoria(CategoriaDTO categoriaDTO)
        {
            if (categoriaDTO is null)
            {
                _logger.LogWarning("Dados inválidos!");
                return BadRequest("Dados inválidos!");
            }

            var categoria = categoriaDTO.ToCategoria();

            var categoriaNova = _uof.CategoriaRepository.Create(categoria);
            _uof.Commit();

            var novaCategoriaDTO = categoria.ToCategoriaDTO();

            return new CreatedAtRouteResult("BuscarCategoriaPorId",
                new { id = novaCategoriaDTO.IdCategoria }, novaCategoriaDTO);
        }

        [HttpPut("{id:int}")]
        public ActionResult<CategoriaDTO> AlterarCategoria(int id, CategoriaDTO categoriaDTO)
        {
            if (id != categoriaDTO.IdCategoria)
            {
                _logger.LogWarning("Dados inválidos!");
                return BadRequest("Dados inválidos!");
            }

            var categoria = categoriaDTO.ToCategoria();

            _uof.CategoriaRepository.Update(categoria);
            _uof.Commit();

            var categoriaAlteradaDTO = categoria.ToCategoriaDTO();

            return Ok(categoriaAlteradaDTO);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<CategoriaDTO> Delete(int id)
        {
            var categoria = _uof.CategoriaRepository.GetId(c => c.IdCategoria == id);

            if (categoria is null)
                return NotFound("Categoria não encontrada!");

            var categoriaExcluida = _uof.CategoriaRepository.Delete(categoria);
            _uof.Commit();

            var categoriaExcluidaDTO = categoriaExcluida.ToCategoriaDTO();

            return Ok(categoriaExcluidaDTO);
        }
    }
}
