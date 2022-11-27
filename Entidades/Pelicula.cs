using Microsoft.EntityFrameworkCore;

class Pelicula
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public bool EnCartelera { get; set; }
    public DateTime FechaEstreno { get; set; }

    // [Unicode(false)]
    public string PosterURL { get; set; }
}