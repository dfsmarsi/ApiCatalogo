namespace APICatalogo.Models;

public class Categoria
{
    public int IdCategoria { get; set; }

    public string? Nome { get; set; }

    public string? UrlImagem { get; set; }

    public List<Produto>? Produtos { get; set; } = new List<Produto>();
}
