using Microsoft.AspNetCore.Mvc;
using SharedApp.Data;
using SharedApp.Models;

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
        public Presentation? Post([FromBody] Presentation value)
        {
            _context.presentations.Add(value);
            _context.SaveChanges();
            return value;
        }

    }
}
