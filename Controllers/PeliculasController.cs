using System.Runtime.CompilerServices;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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

    [HttpGet("/{id:int}")]
    public async Task<ActionResult<PeliculaDTO>> Get(int id)
    {
        var pelicula = await context.Peliculas
        .Include(p => p.Generos.OrderByDescending(g => g.Nombre))
        .Include(p => p.SalasDeCine)
            .ThenInclude(s => s.Cine)
        .Include(p => p.PeliculasActores.Where(pa => pa.Actor.FechaNacimiento.Value.Year >= 1980))
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
    [HttpGet("conprojectto/{id:int}")]
    public async Task<ActionResult<PeliculaDTO>> GetProjectTo(int id)
    {
        var pelicula = await context.Peliculas
        .ProjectTo<PeliculaDTO>(mapper.ConfigurationProvider)
        .FirstOrDefaultAsync(p => p.Id == id);

        if (pelicula is null)
        {
            return NotFound();
        }
        //Para quitar los cines repetidos
        pelicula.Cines = pelicula.Cines.DistinctBy(x => x.Id).ToList();

        return pelicula;
    }

    [HttpGet("cargadoselectivo/{id:int}")]
    public async Task<ActionResult> GetSelectivo(int id)
    {
        var pelicula = await context.Peliculas.Select(p => new
        {
            Id = p.Id,
            Titulo = p.Titulo,
            Generos = p.Generos.OrderByDescending(g => g.Nombre).Select(g => g.Nombre).ToList(),
            CantidadActores = p.PeliculasActores.Count(),
            CantidadCines = p.SalasDeCine.Select(c => c.CineId).Distinct().Count()
        }).FirstOrDefaultAsync(p => p.Id == id);

        if (pelicula is null)
        {
            return NotFound();
        }

        return Ok(pelicula);
    }

    [HttpGet("cargadoexplicito/{id:int}")]
    public async Task<ActionResult<PeliculaDTO>> GetExplicito(int id)
    {
        var pelicula = await context.Peliculas.AsTracking().FirstOrDefaultAsync(p => p.Id == id);

        // await context.Entry(pelicula).Collection(p => p.Generos).LoadAsync();
        var cantidadGeneros = await context.Entry(pelicula).Collection(p => p.Generos).Query().CountAsync();

        if (pelicula is null)
        {
            return NotFound();
        }

        var peliculaDTO = mapper.Map<PeliculaDTO>(pelicula);


        return peliculaDTO;
    }

    //NO es muy utilizado
    [HttpGet("lazyloading/{id:int}")]
    public async Task<ActionResult<List<PeliculaDTO>>> GetLazyLoading(int id)
    {
        var peliculas = await context.Peliculas.AsTracking().ToListAsync();
        foreach (var pelicula in peliculas)
        {
            //cargar los generos de la pelicula
            //problema n+1 100 querys para cargar una consulta
            pelicula.Generos.ToList();
        }

        var peliculasDTOs = mapper.Map<List<PeliculaDTO>>(peliculas);
        return peliculasDTOs;
    }

    [HttpGet("agrupadasporestreno")]
    public async Task<ActionResult> GetAgrupadasPorCartelera()
    {
        var peliculasAgrupadas = await context.Peliculas.GroupBy(p => p.EnCartelera).Select(g => new
        {
            Encartelera = g.Key,
            Conteo = g.Count(),
            Peliculas = g.ToList()
        }).ToListAsync();

        return Ok(peliculasAgrupadas);
    }

    [HttpGet("agrupadasPorCantidadDeGeneros")]
    public async Task<ActionResult> GetAgrupadasPorGeneros()
    {
        var generosAgrupados = await context.Peliculas.GroupBy(p => p.Generos.Count()).Select(g => new
        {
            Conteo = g.Key,
            Titulos = g.Select(x => x.Titulo),
            Generos = g.Select(p => p.Generos).SelectMany(gen => gen).Select(gen => gen.Nombre).Distinct()
        }).ToListAsync();

        return Ok(generosAgrupados);
    }

    [HttpGet("filtrar")]
    public async Task<ActionResult<List<PeliculaDTO>>> Filtrar([FromQuery] PeliculasFiltroDTO peliculasFiltroDTO)
    {
        var peliculasQueryabl = context.Peliculas.AsQueryable();
        if (!string.IsNullOrEmpty(peliculasFiltroDTO.Titulo))
        {
            peliculasQueryabl = peliculasQueryabl.Where(p => p.Titulo.Contains(peliculasFiltroDTO.Titulo));
        }

        if (peliculasFiltroDTO.EnCartelera)
        {
            peliculasQueryabl = peliculasQueryabl.Where(p => p.EnCartelera);
        }

        if (peliculasFiltroDTO.ProximosEstrenos)
        {
            var hoy = DateTime.Today;
            peliculasQueryabl = peliculasQueryabl.Where(p => p.FechaEstreno > hoy);
        }

        if (peliculasFiltroDTO.GeneroId != 0)
        {
            peliculasQueryabl = peliculasQueryabl.Where(p => p.Generos.Select(g => g.Identificador).Contains(peliculasFiltroDTO.GeneroId));
        }

        var peliculas = await peliculasQueryabl.Include(p => p.Generos).ToListAsync();
        return mapper.Map<List<PeliculaDTO>>(peliculas);
    }

    [HttpPost]
    public async Task<ActionResult> Post(PeliculaCreacionDTO peliculaCreacionDTO)
    {
        var pelicula = mapper.Map<Pelicula>(peliculaCreacionDTO);
        pelicula.Generos.ForEach(g => context.Entry(g).State = EntityState.Unchanged);
        pelicula.SalasDeCine.ForEach(s => context.Entry(s).State = EntityState.Unchanged);

        if (pelicula.PeliculasActores is not null)
        {
            for (int i = 0; i < pelicula.PeliculasActores.Count; i++)
            {
                pelicula.PeliculasActores[i].Orden = i + 1;
            }
        }

        context.Add(pelicula);
        await context.SaveChangesAsync();
        return Ok();
    }
}