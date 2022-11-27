using Microsoft.EntityFrameworkCore;

class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //Aplifluente

        //Decimos que queremos que la primary key sea el identificador o una propiedad a nuestra eleccion
        // modelBuilder.Entity<Genero>().HasKey(prop => prop.Identificador);
        // Elegimos la propiedad que queremos 
        // Le decimos que queremos darle una longitud
        modelBuilder.Entity<Genero>().Property(prop => prop.Nombre).HasMaxLength(150).IsRequired();
        //Con ->HasColumnName Cambiamos el nombre de la columna
        // modelBuilder.Entity<Genero>().Property(prop => prop.Nombre).HasMaxLength(150).IsRequired().HasColumnName("NombreGenero");
        //Para cambiar de nombre a la tabla
        // modelBuilder.Entity<Genero>().ToTable(name: "tablaGenero", schema: "Peliculas");

        modelBuilder.Entity<Actor>().Property(prop => prop.Nombre).HasMaxLength(150).IsRequired();
        modelBuilder.Entity<Actor>().Property(prop => prop.FechaNacimiento).HasColumnType("date");

        modelBuilder.Entity<Cine>().Property(prop => prop.Nombre).HasMaxLength(150).IsRequired();

        modelBuilder.Entity<SalaDeCine>().Property(prop => prop.Precio).HasPrecision(precision: 9, scale: 2);
        modelBuilder.Entity<SalaDeCine>().Property(prop => prop.TipoSalaDeCine).HasDefaultValue(TipoSalaDeCine.DosDimensiones);

        modelBuilder.Entity<Pelicula>().Property(prop => prop.Titulo).HasMaxLength(250).IsRequired();
        modelBuilder.Entity<Pelicula>().Property(prop => prop.FechaEstreno).HasColumnType("date");
        //Acepta tipos de caracteres
        modelBuilder.Entity<Pelicula>().Property(prop => prop.PosterURL).HasMaxLength(500).IsUnicode(false);

        modelBuilder.Entity<CineOferta>().Property(prop => prop.ProcentajeDescuento).HasPrecision(precision: 5, scale: 2);
        modelBuilder.Entity<CineOferta>().Property(prop => prop.FechaInicio).HasColumnType("date");
        modelBuilder.Entity<CineOferta>().Property(prop => prop.FechaFin).HasColumnType("date");

    }
    public DbSet<SalaDeCine> SalasDeCine { get; set; }
    public DbSet<CineOferta> CineOfertas { get; set; }
    public DbSet<Pelicula> Peliculas { get; set; }
    public DbSet<Cine> Cines { get; set; }
    public DbSet<Actor> Actores { get; set; }
    public DbSet<Genero> Generos { get; set; }
}