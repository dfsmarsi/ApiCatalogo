using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APICatalogo.Mappings
{
    public class ProdutoMap : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable("produtos");

            builder.HasKey(x => x.IdProduto);

            builder.Property(x => x.IdProduto)
                .HasColumnName("idproduto")
                .UseSerialColumn();

            builder.Property(x => x.Nome)
                .HasColumnName("nome")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Descricao)
                .HasColumnName("descricao")
                .HasMaxLength(300);

            builder.Property(x => x.Preco)
                .HasColumnName("preco")
                .HasColumnType("NUMERIC(10,2)")
                .IsRequired();

            builder.Property(x => x.UrlImagem)
                .HasColumnName("urlimagem");

            builder.Property(x => x.Estoque)
                .HasColumnName("qtdeestoque")
                .IsRequired();

            builder.Property(x => x.DataCadastro)
                .HasColumnName("datacadastro")
                .HasColumnType("timestamptz")
                .IsRequired();

            builder.Property(x => x.IdCategoria)
                .HasColumnName("idcategoria");

            builder
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Produtos)
                .HasForeignKey(p => p.IdCategoria)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
