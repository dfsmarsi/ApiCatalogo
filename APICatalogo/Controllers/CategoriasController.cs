using APICatalogo.Context;
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
        private readonly IRepository<Categoria> _repository;
        private readonly ILogger _logger;

        public CategoriasController(IRepository<Categoria> repository, ILogger<CategoriasController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))] // Filtro personalisado para gerar logs
        public ActionResult<IEnumerable<Categoria>> BuscarTodasAsCategorias()
        {
            var categorias = _repository.GetAll;

            return Ok(categorias);
        }

        [HttpGet("{id:int}", Name = "BuscarCategoriaPorId")]
        public ActionResult<Categoria> BuscarCategoriaPorId(int id)
        {
            var categoria = _repository.GetId(c=> c.IdCategoria == id);

            if (categoria is null)
            {
                _logger.LogWarning("----------------------- Get API/CategoriaId Categoria não encontrada  ----------------------");
                return NotFound($"Categoria código {id} não encontrada!");
            }
                
            return Ok(categoria);
        }

        [HttpPost]
        public ActionResult CriarCategoria(Categoria categoria)
        {
            if (categoria is null)
            {
                _logger.LogWarning("Dados inválidos!");
                return BadRequest("Dados inválidos!");
            }

            var categoriaNova = _repository.Create(categoria);

            return new CreatedAtRouteResult("BuscarCategoriaPorId",
                new { id = categoriaNova.IdCategoria }, categoriaNova);
        }

        [HttpPut("{id:int}")]
        public ActionResult AlterarCategoria(int id, Categoria categoria)
        {
            if (id != categoria.IdCategoria)
            {
                _logger.LogWarning("Dados inválidos!");
                return BadRequest("Dados inválidos!");
            }

            _repository.Update(categoria);
            
            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var categoria = _repository.GetId(c => c.IdCategoria == id);

            if (categoria is null)
                return NotFound("Categoria não encontrada!");

            var categoriaExcluida =  _repository.Delete(categoria);

            return Ok(categoriaExcluida);
        }
    }
}
