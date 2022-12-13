using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

//Para darle nombre a la tabla y el eschema
// [Table("tablaGeneros", Schema = "peliculas")]
namespace ef_core_peliculas.Entidades;
public class Genero
{
    // [Key]
    public int Identificador { get; set; }
    // public int Id { get; set; }

    // [StringLength(150)]
    // [Required]
    // [Column("NombreGenero")] -> Cambia el nombre de la columna
    public string Nombre { get; set; }
    public HashSet<Pelicula> Peliculas { get; set; } //RElacion muchos a muchos no controlada
    public bool EstaBorrado { get; set; }
}