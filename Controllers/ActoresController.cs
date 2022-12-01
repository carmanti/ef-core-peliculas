using System.Security.Cryptography;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ef_core_peliculas.DTOS;
using ef_core_peliculas.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ef_core_peliculas.Controllers;

[ApiController]
[Route("api/actores")]
public class ActoresController : ControllerBase
{
    private readonly ApplicationDbContext context;
    private readonly IMapper mapper;

    public ActoresController(ApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    [HttpGet]
    // public async Task<ActionResult> Get() -> Para mapear tipos anonimos
    public async Task<IEnumerable<ActorDTO>> Get()
    {
        //Se mapea a un tipo anonimo
        // var actores = await context.Actores.Select(a => new { Id = a.Id, Nombre = a.Nombre }).ToListAsync();
        // var actores = await context.Actores.Select(a => new ActorDTO
        // {    
        //     Id = a.Id,
        //     Nombre = a.Nombre
        // }).ToListAsync();
        // return actores;
        //Con AutoMapper ya no necesitamos el Select
        return await context.Actores.ProjectTo<ActorDTO>(mapper.ConfigurationProvider).ToListAsync();
    }
}