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
	public class PresentationController : ControllerBase
	{
		private readonly AppDbContext _context;
		public PresentationController(AppDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public IEnumerable<Presentation> Get()
		{
			return _context.presentations.ToArray();
		}
		[HttpPost]
		public void Post([FromBody] Presentation value)
		{
			_context.presentations.Add(value);
			_context.SaveChanges();
		}

	}
}
