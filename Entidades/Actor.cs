using System.ComponentModel.DataAnnotations.Schema;

class Actor
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Biografia { get; set; }

    // [Column(TypeName = "Date")] -> Le damos formato a la fecha
    public DateTime? FechaNacimiento { get; set; }
}