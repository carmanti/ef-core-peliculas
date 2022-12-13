namespace ef_core_peliculas.DTOS;

public class ActorCreacionDTO
{
    public string Nombre { get; set; }
    public string Biografia { get; set; }
    public DateTime? FechaNacimiento { get; set; }
}