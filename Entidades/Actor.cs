using System.ComponentModel.DataAnnotations.Schema;

namespace ef_core_peliculas.Entidades;
public class Actor
{
    public int Id { get; set; }
    public string _nombre { get; set; }
    public string Nombre
    {
        get
        {
            return _nombre;
        }
        set
        {
            _nombre = string.Join(' ', value.Split(' ').Select(x => x[0].ToString().ToUpper() + x.Substring(1).ToLower()).ToArray());
        }
    }
    public string Biografia { get; set; }

    // [Column(TypeName = "Date")] -> Le damos formato a la fecha
    public DateTime? FechaNacimiento { get; set; }
    public HashSet<PeliculaActor> PeliculasActores { get; set; }
}