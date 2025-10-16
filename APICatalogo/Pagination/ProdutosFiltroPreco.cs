namespace APICatalogo.Pagination
{
    public class ProdutosFiltroPreco : PaginationParameters
    {
        public decimal? Preco { get; set; }
        public string? PrecoCriterio { get; set; }
    }
}
