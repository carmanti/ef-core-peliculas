using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ef_core_peliculas.Entidades;
public class SalaDeCineConfig : IEntityTypeConfiguration<SalaDeCine>
{
    public void Configure(EntityTypeBuilder<SalaDeCine> builder)
    {
        builder.Property(prop => prop.Precio).HasPrecision(precision: 9, scale: 2);
        builder.Property(prop => prop.TipoSalaDeCine).HasDefaultValue(TipoSalaDeCine.DosDimensiones);
    }
}