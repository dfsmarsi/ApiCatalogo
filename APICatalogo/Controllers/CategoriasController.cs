using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
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
        public ActionResult<IEnumerable<Categoria>> BuscarTodasAsCategorias()
        {
            var categorias = _context.Categorias.ToList();

            if (categorias == null || !categorias.Any())
                return NotFound("Não há Categorias!");

            return categorias;
        }

        [HttpGet("{id:int}", Name= "ObterCategoriaPorId")]
        public ActionResult<Categoria> BuscarCategoriaPorId(int id)
        {
            var categoria = _context.Categorias.FirstOrDefault(c => c.IdCategoria == id);

            if (categoria is null)
                return NotFound($"Categoria código {id} não encontrada!");

            return categoria;
        }

        [HttpPost]
        public ActionResult GravarCategoria(Categoria categoria)
        {
            if (categoria is null)
                return BadRequest();

            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterCategoriaPorId", 
                new { id = categoria.IdCategoria }, categoria);
        }
    }
}
