using Microsoft.EntityFrameworkCore;
using SharedApp.Models;

namespace SharedApp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<CatalogMusic> catalogMusics { get; set; }
        public DbSet<Artist> artists { get; set; }
        public DbSet<Genre> genres { get; set; }
        public DbSet<Format> formats { get; set; }
        public DbSet<Presentation> presentations { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Server=tcp:imagina.database.windows.net,1433;Initial Catalog=catalog-microservice;Persist Security Info=False;User ID=imagina;Password=Vaca$Loca69;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            optionsBuilder.UseSqlite("Data Source=Catalog.db;", connection => connection.MigrationsAssembly("Catalog.API"));
            

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Artist>()                                
                .Ignore(p => p.CatalogMusics);

            modelBuilder.Entity<Artist>()
                .Property(f => f.Id)
                .ValueGeneratedOnAdd();

			modelBuilder.Entity<Genre>()
                .Ignore(p => p.CatalogMusics);

            modelBuilder.Entity<Genre>()
                .Property(f => f.Id)
                .ValueGeneratedOnAdd();			

			modelBuilder.Entity<Format>()
		        .Property(f => f.Id)
		        .ValueGeneratedOnAdd();

			modelBuilder.Entity<Presentation>()                
				.Ignore(p => p.CatalogMusics);

			modelBuilder.Entity<Presentation>()
		        .Property(f => f.Id)
		        .ValueGeneratedOnAdd();
		}
    }
}
