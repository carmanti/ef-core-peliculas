using AutoMapper;
using ef_core_peliculas.DTOS;
using ef_core_peliculas.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ef_core_peliculas.Controllers;

[ApiController]
[Route("api/peliculas")]
public class PeliculasController : ControllerBase
{
    private readonly ApplicationDbContext context;
    private readonly IMapper mapper;

    public PeliculasController(ApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PeliculaDTO>> Get(int id)
    {
        var pelicula = await context.Peliculas
        .Include(p => p.Generos)
        .Include(p => p.SalasDeCine)
            .ThenInclude(s => s.Cine)
        .Include(p => p.PeliculasActores)
            .ThenInclude(pa => pa.Actor)
        .FirstOrDefaultAsync(p => p.Id == id);

        if (pelicula is null)
        {
            return NotFound();
        }

        var peliculaDTO = mapper.Map<PeliculaDTO>(pelicula);

        //Para quitar los cines repetidos
        peliculaDTO.Cines = peliculaDTO.Cines.DistinctBy(x => x.Id).ToList();

        return peliculaDTO;
    }
}