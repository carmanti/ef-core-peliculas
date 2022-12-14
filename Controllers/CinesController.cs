using System.Collections.Generic;
using System.Runtime.InteropServices;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ef_core_peliculas.DTOS;
using ef_core_peliculas.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;

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

    [HttpPost]
    public async Task<ActionResult> Post()
    {
        var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
        var ubicacionCine = geometryFactory.CreatePoint(new Coordinate(-69.896979, 18.476276));
        var cine = new Cine()
        {
            Nombre = "Mi cine",
            Ubicacion = ubicacionCine,
            CineOferta = new CineOferta()
            {
                PorcentajeDescuento = 5,
                FechaInicio = DateTime.Today,
                FechaFin = DateTime.Today.AddDays(7)
            },
            SalasDeCine = new HashSet<SalaDeCine>(){
                new SalaDeCine(){
                    Precio=200,
                TipoSalaDeCine = TipoSalaDeCine.DosDimensiones
                },
                new SalaDeCine(){
                    Precio=350,
                TipoSalaDeCine = TipoSalaDeCine.TresDimensiones
                }
            },
        };

        context.Add(cine);
        await context.SaveChangesAsync();
        return Ok();



    }
}