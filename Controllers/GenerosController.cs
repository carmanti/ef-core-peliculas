using ef_core_peliculas.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ef_core_peliculas.Controllers;

[ApiController]
[Route("api/generos")]
public class GenerosController : ControllerBase
{
    private readonly ApplicationDbContext context;

    public GenerosController(ApplicationDbContext context)
    {
        this.context = context;
    }
    // Traer todos los registros
    [HttpGet]
    public async Task<IEnumerable<Genero>> Get()
    {
        return await context.Generos.OrderBy(p => p.Nombre).ToListAsync();
    }

    // Buscar por id
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Genero>> Get(int id)
    {
        var genero = await context.Generos.FirstOrDefaultAsync(p => p.Identificador == id);
        if (genero is null)
        {
            return NotFound();
        }
        return genero;
    }

    // Buscar el primer elemento
    [HttpGet("primer")]
    public async Task<ActionResult<Genero>> Primer()
    {
        var genero = await context.Generos.FirstOrDefaultAsync(p => p.Nombre.StartsWith("C"));
        if (genero is null)
        {
            return NotFound();
        }
        return genero;
    }

    //Filtrar con Where
    [HttpGet("filtrar")]
    public async Task<IEnumerable<Genero>> Filtrar(string nombre)
    {
        return await context.Generos.Where(
        // p => p.Nombre.StartsWith("C") || p.Nombre.StartsWith("A")).ToListAsync();
        p => p.Nombre.Contains(nombre)).ToListAsync();
    }


    //PAginacion
    [HttpGet("paginacion")]
    public async Task<ActionResult<IEnumerable<Genero>>> GetPaginacion(int pagina = 1)
    {
        var cantidadResgistrosPorPagina = 2;
        var generos = await context.Generos
        .Skip((pagina - 1) * cantidadResgistrosPorPagina)
        .Take(cantidadResgistrosPorPagina).ToListAsync();

        return generos;
    }

}