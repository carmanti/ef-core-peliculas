using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace ef_core_peliculas.Entidades;
public class Cine
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    // [Precision(precison: 9, scale: 2)] -> Cuantos campos almacena nuestro campo (2 decimales y 9 digitos)
    // public decimal Precio { get; set; }
    public Point Ubicacion { get; set; }
    public CineOferta CineOferta { get; set; } // Propiedad de navegacion
    public HashSet<SalaDeCine> SalasDeCine { get; set; }
}