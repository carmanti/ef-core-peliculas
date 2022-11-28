using Microsoft.EntityFrameworkCore;
namespace ef_core_peliculas.Entidades;
public class Pelicula
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public bool EnCartelera { get; set; }
    public DateTime FechaEstreno { get; set; }

    // [Unicode(false)]
    public string PosterURL { get; set; }
    public List<Genero> Generos { get; set; }//Relacion muchos a muchos no controlada Pelicula Genero
    public HashSet<SalaDeCine> SalasDeCine { get; set; }//Relacion muchos a muchos no controlada Pelicula Sala De Cine
    public HashSet<PeliculaActor> PeliculasActores { get; set; }
}