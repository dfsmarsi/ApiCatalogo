using APICatalogo.Context;
using APICatalogo.Filters;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))] // Filtro personalisado para gerar logs
        public ActionResult<IEnumerable<Categoria>> BuscarTodasAsCategorias()
        {
            try
            {
                var categorias = _context.Categorias.AsNoTracking().ToList();

                if (categorias is null)
                    return NotFound("Não há Categorias!");

                return categorias;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema na sua requisição!");
            }
        }

        [HttpGet("{id:int}", Name= "BuscarCategoriaPorId")]
        public ActionResult<Categoria> BuscarCategoriaPorId(int id)
        {
            try
            {
                var categoria = _context.Categorias.FirstOrDefault(c => c.IdCategoria == id);

                if (categoria is null)
                    return NotFound($"Categoria código {id} não encontrada!");

                return categoria;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema na sua requisição!");
            }
        }

        [HttpGet("CategoriasComProdutos")]
        public ActionResult<IEnumerable<Categoria>> BuscarCategoriasComProdutos()
        {
            try
            {
                var categorias = _context.Categorias.Include(p => p.Produtos).Where(c => c.IdCategoria <= 10).ToList();

                if (categorias is null)
                    return NotFound("Não há categorias!");

                return categorias;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema na sua requisição!");
            }
        }

        [HttpPost]
        public ActionResult GravarCategoria(Categoria categoria)
        {
            if (categoria is null)
                return BadRequest();

            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            return new CreatedAtRouteResult("BuscarCategoriaPorId", 
                new { id = categoria.IdCategoria }, categoria);
        }

        [HttpPut("{id:int}")]
        public ActionResult AlterarCategoria(int id, Categoria categoria)
        {
            if (id != categoria.IdCategoria)
                return BadRequest();

            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id) 
        {
            var categoria = _context.Categorias.FirstOrDefault(c => c.IdCategoria == id);

            if(categoria is null)
                return NotFound("Categoria não encontrada!");

            _context.Categorias.Remove(categoria);
            _context.SaveChanges();

            return Ok(categoria);
        }
    }
}
