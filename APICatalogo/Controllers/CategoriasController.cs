using APICatalogo.DTO;
using APICatalogo.DTO.Mappings;
using APICatalogo.Filters;
using APICatalogo.Pagination;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly ILogger<CategoriasController> _logger;

        public CategoriasController(IUnitOfWork uof, ILogger<CategoriasController> logger)
        {
            _uof = uof;
            _logger = logger;
        }

        [HttpGet("paginacao")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasPaginacao(
            [FromQuery] CategoriaParameters categoriaParameters)
        {
            var categorias = await _uof.CategoriaRepository.GetCategoriasPaginadasAsync(categoriaParameters);
            return ObterCategoriasFiltradas(categorias);
        }

        private ActionResult<IEnumerable<CategoriaDTO>> ObterCategoriasFiltradas(PagedList<Models.Categoria> categorias)
        {
            var metadata = new
            {
                categorias.TotalCount,
                categorias.PageSize,
                categorias.CurrentPage,
                categorias.TotalPages,
                categorias.HasNext,
                categorias.HasPrevious
            };
            Response.Headers.Add("X-Pagination",
                System.Text.Json.JsonSerializer.Serialize(metadata));
            var categoriasDTO = categorias.ToCategoriaDTOList();
            return Ok(categoriasDTO);
        }

        [HttpGet("filtro/nome")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasFiltroPorNome(
            [FromQuery] CategoriaFiltroNome categoriaFiltroNome)
        {
            var categorias = await _uof.CategoriaRepository.GetCategoriasFiltroPorNomeAsync(categoriaFiltroNome);
            
            return ObterCategoriasFiltradas(categorias);
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))] // Filtro personalisado para gerar logs
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> BuscarTodasAsCategorias()
        {
            var categorias = await _uof.CategoriaRepository.GetAllAsync();

            if(categorias is null)
                return NotFound("Nenhuma categoria cadastrada!");

            var categoriasDTO = categorias.ToCategoriaDTOList();

            return Ok(categoriasDTO);
        }

        [HttpGet("{id:int}", Name = "BuscarCategoriaPorId")]
        public async Task<ActionResult<CategoriaDTO>> BuscarCategoriaPorId(int id)
        {
            var categoria = await _uof.CategoriaRepository.GetIdAsync(c=> c.IdCategoria == id);

            if (categoria is null)
            {
                _logger.LogWarning("----------------------- Get API/CategoriaId Categoria não encontrada  ----------------------");
                return NotFound($"Categoria código {id} não encontrada!");
            }

            var categoriaDTO = categoria.ToCategoriaDTO();

            return Ok(categoriaDTO);
        }

        [HttpPost]
        public async Task<ActionResult<CategoriaDTO>> CriarCategoria(CategoriaDTO categoriaDTO)
        {
            if (categoriaDTO is null)
            {
                _logger.LogWarning("Dados inválidos!");
                return BadRequest("Dados inválidos!");
            }

            var categoria = categoriaDTO.ToCategoria();

            var categoriaNova = _uof.CategoriaRepository.Create(categoria);
            await _uof.CommitAsync();

            var novaCategoriaDTO = categoria.ToCategoriaDTO();

            return new CreatedAtRouteResult("BuscarCategoriaPorId",
                new { id = novaCategoriaDTO.IdCategoria }, novaCategoriaDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> AlterarCategoria(int id, CategoriaDTO categoriaDTO)
        {
            if (id != categoriaDTO.IdCategoria)
            {
                _logger.LogWarning("Dados inválidos!");
                return BadRequest("Dados inválidos!");
            }

            var categoria = categoriaDTO.ToCategoria();

            _uof.CategoriaRepository.Update(categoria);
            await _uof.CommitAsync();

            var categoriaAlteradaDTO = categoria.ToCategoriaDTO();

            return Ok(categoriaAlteradaDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> Delete(int id)
        {
            var categoria = await _uof.CategoriaRepository.GetIdAsync(c => c.IdCategoria == id);

            if (categoria is null)
                return NotFound("Categoria não encontrada!");

            var categoriaExcluida = _uof.CategoriaRepository.Delete(categoria);
            await _uof.CommitAsync();

            var categoriaExcluidaDTO = categoriaExcluida.ToCategoriaDTO();

            return Ok(categoriaExcluidaDTO);
        }
    }
}
