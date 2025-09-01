using APICatalogo.Models;

namespace APICatalogo.DTO.Mappings
{
    public static class CategoriaDTOMappingExtensions
    {
        public static CategoriaDTO? ToCategoriaDTO(this Categoria categoria)
        {
            if (categoria is null)
                return null;

            return new CategoriaDTO
            {
                IdCategoria = categoria.IdCategoria,
                Nome = categoria.Nome,
                UrlImagem = categoria.UrlImagem
            };
        }

        public static Categoria? ToCategoria(this CategoriaDTO categoriaDTO)
        {
            if (categoriaDTO is null)
                return null;

            return new Categoria
            {
                IdCategoria = categoriaDTO.IdCategoria,
                Nome = categoriaDTO.Nome,
                UrlImagem = categoriaDTO.UrlImagem
            };
        }

        public static IEnumerable<CategoriaDTO> ToCategoriaDTOList(this IEnumerable<Categoria> categorias)
        {
            if (categorias is null || !categorias.Any())
                return new List<CategoriaDTO>();

            return categorias.Select(categoria => new CategoriaDTO
            {
                IdCategoria = categoria.IdCategoria,
                Nome = categoria.Nome,
                UrlImagem = categoria.UrlImagem
            }).ToList();
        }
    }
}
