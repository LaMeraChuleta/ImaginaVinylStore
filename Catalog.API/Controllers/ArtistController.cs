using SharedApp.Models;
using Microsoft.AspNetCore.Mvc;
using SharedApp.Data;

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
            return _context.Artists.ToArray();
        }
        [HttpPost]
        public Artist Post([FromBody] Artist value)
        {
            _context.Artists.Add(value);
            _context.SaveChanges();
            return value;
        }
    }
}
