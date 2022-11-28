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

    [HttpGet]
    public async Task<IEnumerable<Genero>> Get()
    {
        return await context.Generos.ToListAsync();
    }

}