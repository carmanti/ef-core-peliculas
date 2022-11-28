using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ef_core_peliculas.Entidades;
public class PeliculaActorConfig : IEntityTypeConfiguration<PeliculaActor>
{
    public void Configure(EntityTypeBuilder<PeliculaActor> builder)
    {
        builder.HasKey(prop => new { prop.ActorId, prop.PeliculaId });
        builder.Property(prop => prop.Personaje).HasMaxLength(150);

    }
}