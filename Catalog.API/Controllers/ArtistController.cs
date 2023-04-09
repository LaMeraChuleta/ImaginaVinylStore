using Catalog.API.Data;
using Catalog.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Catalog.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ArtistController : ControllerBase
	{
		private readonly AppDbContext _context;
		public ArtistController(AppDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public IEnumerable<Artist> Get()
		{
			return _context.artists.ToArray();
		}
		[HttpPost]
		public void Post([FromBody] Artist value)
		{
			_context.artists.Add(value);
			_context.SaveChanges();
		}
	}
}
