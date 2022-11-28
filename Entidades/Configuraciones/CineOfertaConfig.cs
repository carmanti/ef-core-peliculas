using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ef_core_peliculas.Entidades;
public class CineOfertaConfig : IEntityTypeConfiguration<CineOferta>
{
    public void Configure(EntityTypeBuilder<CineOferta> builder)
    {
        builder.Property(prop => prop.PorcentajeDescuento).HasPrecision(precision: 5, scale: 2);
        // builder.Property(prop => prop.FechaInicio).HasColumnType("date");
        // builder.Property(prop => prop.FechaFin).HasColumnType("date");

    }
}