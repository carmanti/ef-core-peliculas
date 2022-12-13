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

    [HttpPost]
    public async Task<ActionResult> Post(ActorCreacionDTO actorCreacionDTO)
    {
        var actor = mapper.Map<Actor>(actorCreacionDTO);
        context.Add(actor);
        await context.SaveChangesAsync();
        return Ok();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(ActorCreacionDTO actorCreacionDTO, int id)
    {
        var actorDB = await context.Actores.AsTracking().FirstOrDefaultAsync(a => a.Id == id);

        if (actorDB is null)
        {
            return NotFound();
        }

        actorDB = mapper.Map(actorCreacionDTO, actorDB);
        await context.SaveChangesAsync();
        return Ok();
    }

    [HttpPut("desconectado/{id:int}")]
    public async Task<ActionResult> PutDesconectado(ActorCreacionDTO actorCreacionDTO, int id)
    {
        var existeActor = await context.Actores.AnyAsync(a => a.Id == id);

        if (!existeActor)
        {
            return NotFound();
        }

        var actor = mapper.Map<Actor>(actorCreacionDTO);
        actor.Id = id;
        context.Update(actor);
        await context.SaveChangesAsync();
        return Ok();
    }


}