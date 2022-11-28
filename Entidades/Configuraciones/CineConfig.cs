using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ef_core_peliculas.Entidades;
public class CineConfig : IEntityTypeConfiguration<Cine>
{
    public void Configure(EntityTypeBuilder<Cine> builder)
    {
        builder.Property(prop => prop.Nombre).HasMaxLength(150).IsRequired();
    }
}