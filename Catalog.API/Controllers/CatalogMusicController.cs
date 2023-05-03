using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedApp.Data;
using SharedApp.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogMusicController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CatalogMusicController(AppDbContext context) 
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<CatalogMusic> Get()
        {            
            return _context.catalogMusics                
                .Include(x => x.Artist)                    
                .Include(x => x.Genre) 
                .Include(x => x.Presentation)     
                .Include(x => x.Format)
                .ToArray();
        }
		[HttpGet("{id}")]
		public CatalogMusic Get(int id)
		{
			return _context.catalogMusics.Find(id);
		}
		[HttpPost]
		public void Post([FromBody] CatalogMusic value)
		{
			_context.catalogMusics.Add(value);
			_context.SaveChanges();
		}			    
    }
}
