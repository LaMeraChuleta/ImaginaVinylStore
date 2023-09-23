using Microsoft.EntityFrameworkCore;
using SharedApp.Models;

namespace SharedApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<AudioCatalog> AudioCatalog { get; set; }
    public DbSet<MusicCatalog> MusicCatalog { get; set; }
    public DbSet<Artist> Artist { get; set; }
    public DbSet<Genre> Genre { get; set; }
    public DbSet<Format> Format { get; set; }
    public DbSet<ShopCart> ShopCart { get; set; }
    public DbSet<ImageCatalog> ImageCatalog { get; set; }
    public DbSet<ImageAudio> ImageAudio { get; set; }
    public DbSet<ImageArtist> ImageArtist { get; set; }
    public DbSet<Presentation> Presentation { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        const string connectionString =
            "Server=localhost;Database=test;User Id=sa;Password=VacaLoca69;TrustServerCertificate=True;";
        optionsBuilder.UseSqlServer(connectionString, builder => builder.EnableRetryOnFailure());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MusicCatalog>()
            .HasOne<Presentation>(s => s.Presentation)
            .WithMany(g => g.CatalogMusics)
            .HasForeignKey(s => s.PresentationId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<MusicCatalog>()
            .HasOne<Genre>(s => s.Genre)
            .WithMany(g => g.CatalogMusics)
            .HasForeignKey(s => s.GenreId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<MusicCatalog>()
            .HasOne<Artist>(s => s.Artist)
            .WithMany(g => g.CatalogMusics)
            .HasForeignKey(s => s.ArtistId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<ShopCart>()
            .HasOne(x => x.CatalogMusic)
            .WithMany()
            .HasForeignKey(item => item.MusicCatalogId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Presentation>()
            .HasOne(p => p.Format)
            .WithMany(f => f.Presentations)
            .HasForeignKey(p => p.FormatId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Format>()
            .HasMany(f => f.Presentations)
            .WithOne(p => p.Format)
            .HasForeignKey(p => p.FormatId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Presentation>()
            .HasMany(p => p.CatalogMusics) // Una Presentation tiene muchos MusicCatalogs
            .WithOne(m => m.Presentation)  // Un MusicCatalog pertenece a una Presentation
            .HasForeignKey(m => m.PresentationId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<ImageCatalog>()
            .Property(i => i.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<ImageCatalog>()
            .Ignore(i => i.MusicCatalog);

        modelBuilder.Entity<ImageArtist>()
            .Ignore(i => i.Artist);

        modelBuilder.Entity<ImageAudio>()
            .Ignore(i => i.AudioCatalog);

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