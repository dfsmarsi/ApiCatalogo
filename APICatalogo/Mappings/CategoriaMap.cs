using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APICatalogo.Mappings
{
    public class CategoriaMap : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.ToTable("categorias");

            builder.Property(x => x.IdCategoria).HasColumnName("idcategoria").UseSerialColumn();
            builder.Property(x => x.Nome).HasColumnName("nome").IsRequired().HasMaxLength(100);
            builder.Property(x => x.UrlImagem).HasColumnName("urlimagem");

            builder.HasKey(x => x.IdCategoria);
            builder
                .HasMany(c => c.Produtos)
                .WithOne(p => p.Categoria)
                .HasForeignKey(p => p.IdCategoria)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
