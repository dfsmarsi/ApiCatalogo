namespace APICatalogo.Models;

public class Produto
{
    public int IdProduto { get; set; }

    public string? Nome { get; set; }

    public string? Descricao { get; set; }

    public decimal Preco { get; set; }

    public string? UrlImagem { get; set; }

    public float Estoque { get; set; }

    public DateTime DataCadastro { get; set; }

    public int IdCategoria { get; set; }

    public Categoria? Categoria { get; set; }

}
