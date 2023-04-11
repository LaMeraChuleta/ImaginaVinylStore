using Duende.IdentityServer.EntityFramework.Options;
using Identity.API.Models;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Identity.API.Data
{
	public class AppDbContext : ApiAuthorizationDbContext<UserStore>
	{
		public AppDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions) 
			: base(options, operationalStoreOptions)
		{
			
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			//optionsBuilder.UseSqlServer(@"Server=tcp:imagina.database.windows.net,1433;Initial Catalog=catalog-microservice;Persist Security Info=False;User ID=imagina;Password=Vaca$Loca69;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
			optionsBuilder.UseSqlite("Data Source=Catalog.db;");
		
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}
	}
}
