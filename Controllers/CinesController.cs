using System.Runtime.InteropServices;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ef_core_peliculas.DTOS;
using ef_core_peliculas.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;

namespace ef_core_peliculas.Controllers;

[ApiController]
[Route("api/cines")]
public class CinesController : ControllerBase
{
    private readonly ApplicationDbContext context;
    private readonly IMapper mapper;

    public CinesController(ApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    [HttpGet]
    public async Task<IEnumerable<CineDTO>> Get()
    {
        // return await context.Cines.ToListAsync(); -> Esto da errorm
        return await context.Cines.ProjectTo<CineDTO>(mapper.ConfigurationProvider).ToListAsync();
    }

    [HttpGet("cercanos")]
    public async Task<ActionResult> Get(double latitud, double longitud)
    {
        var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
        var miUbicacion = geometryFactory.CreatePoint(new NetTopologySuite.Geometries.Coordinate(longitud, latitud));
        var distanciaMaximaEnMetros = 2000;
        var cines = await context.Cines.OrderBy(c => c.Ubicacion.Distance(miUbicacion)).Where(c => c.Ubicacion.IsWithinDistance(miUbicacion, distanciaMaximaEnMetros))
        .Select(c => new
        {
            Nombre = c.Nombre,
            Distancia = Math.Round(c.Ubicacion.Distance(miUbicacion))
        }).ToListAsync();

        return Ok(cines);
    }
}