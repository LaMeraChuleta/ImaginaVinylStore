using Microsoft.EntityFrameworkCore;
using SharedApp.Models;

namespace SharedApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<MusicCatalog> MusicCatalogs { get; set; }
    public DbSet<Artist> Artists { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Format> Formats { get; set; }
    public DbSet<ImageCatalog> ImagesCatalog { get; set; }
    public DbSet<ImageArtist> ImageArtists { get; set; }
    public DbSet<Presentation> Presentations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        const string connectionString =
            "Server=localhost;Database=test;User Id=sa;Password=VacaLoca69;TrustServerCertificate=True;";
        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MusicCatalog>()
            .HasOne<Presentation>(s => s.Presentation)
            .WithMany(g => g.CatalogMusics)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<ImageCatalog>()
            .Property(i => i.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<ImageCatalog>()
            .Ignore(i => i.MusicCatalog);

        modelBuilder.Entity<ImageArtist>()
            .Ignore(i => i.Artist);

        modelBuilder.Entity<Artist>()
            .Ignore(a => a.CatalogMusics);

        modelBuilder.Entity<Artist>()
            .Property(a => a.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Genre>()
            .Ignore(g => g.CatalogMusics);

        modelBuilder.Entity<Genre>()
            .Property(g => g.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Format>()
            .Ignore(f => f.Presentations);

        modelBuilder.Entity<Format>()
            .Property(f => f.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Presentation>()
            .Property(f => f.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Presentation>()
            .Ignore(p => p.CatalogMusics);
    }
}