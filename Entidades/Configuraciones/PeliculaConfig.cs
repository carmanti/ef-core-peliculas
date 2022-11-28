using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ef_core_peliculas.Entidades;
public class PeliculaConfig : IEntityTypeConfiguration<Pelicula>
{
    public void Configure(EntityTypeBuilder<Pelicula> builder)
    {
        builder.Property(prop => prop.Titulo).HasMaxLength(250).IsRequired();
        // builder.Property(prop => prop.FechaEstreno).HasColumnType("date");
        //Acepta tipos de caracteres
        builder.Property(prop => prop.PosterURL).HasMaxLength(500).IsUnicode(false);
    }
}