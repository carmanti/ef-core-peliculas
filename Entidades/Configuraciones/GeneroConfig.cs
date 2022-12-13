using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ef_core_peliculas.Entidades;
public class GeneroConfig : IEntityTypeConfiguration<Genero>
{
    public void Configure(EntityTypeBuilder<Genero> builder)
    {
        //El apifluente lo traemos aca para orden en el codigo

        //Decimos que queremos que la primary key sea el identificador o una propiedad a nuestra eleccion
        builder.HasKey(prop => prop.Identificador);
        // Elegimos la propiedad que queremos 
        // Le decimos que queremos darle una longitud
        builder.Property(prop => prop.Nombre).HasMaxLength(150).IsRequired();
        //Con ->HasColumnName Cambiamos el nombre de la columna
        // modelBuilder.Entity<Genero>().Property(prop => prop.Nombre).HasMaxLength(150).IsRequired().HasColumnName("NombreGenero");
        //Para cambiar de nombre a la tabla
        // modelBuilder.Entity<Genero>().ToTable(name: "tablaGenero", schema: "Peliculas");

        //Filtros a nivel de modelo
        builder.HasQueryFilter(g => !g.EstaBorrado);
    }
}