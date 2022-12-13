using AutoMapper;
using ef_core_peliculas.DTOS;
using ef_core_peliculas.Entidades;

namespace ef_core_peliculas.Servicios;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Actor, ActorDTO>();
        CreateMap<Cine, CineDTO>()
        .ForMember(dto => dto.Latitud, ent => ent.MapFrom(prop => prop.Ubicacion.Y))
        .ForMember(dto => dto.Longitud, ent => ent.MapFrom(prop => prop.Ubicacion.X));


        CreateMap<Genero, GeneroDTO>();

        //Mapeos compuestos o personalizados
        //Sin projectTo
        CreateMap<Pelicula, PeliculaDTO>()
        .ForMember(dto => dto.Cines, ent => ent.MapFrom(prop => prop.SalasDeCine.Select(s => s.Cine)))
        .ForMember(dto => dto.Actores, ent => ent.MapFrom(prop => prop.PeliculasActores.Select(s => s.Actor)));

        //COn projectTo
        // CreateMap<Pelicula, PeliculaDTO>()
        // .ForMember(dto => dto.Generos, ent => ent.MapFrom(prop => prop.Generos.OrderByDescending(g => g.Nombre)))
        // .ForMember(dto => dto.Cines, ent => ent.MapFrom(prop => prop.SalasDeCine.Select(s => s.Cine)))
        // .ForMember(dto => dto.Actores, ent => ent.MapFrom(prop => prop.PeliculasActores.Select(s => s.Actor)));

        CreateMap<PeliculaCreacionDTO, Pelicula>()
        .ForMember(ent => ent.Generos, dto => dto.MapFrom(campo => campo.Generos.Select(id => new Genero() { Identificador = id })));

        CreateMap<ActorCreacionDTO, Actor>();
    }
}