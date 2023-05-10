using Azure.Storage.Blobs;
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
        private readonly BlobContainerClient _blobClient;
        public CatalogMusicController(AppDbContext context, IConfiguration config)
        {
            _context = context;                                    
            _blobClient = new BlobContainerClient(config["BlobConnectionString"], config["BlobContainerName"]);
        }

        [HttpGet]
        public IEnumerable<MusicCatalog> Get()
        {
            return _context.catalogMusics
                .Include(x => x.Artist)
                .Include(x => x.Genre)
                .Include(x => x.Presentation)
                .Include(x => x.Format)
                .Include(x => x.Images)
                .ToArray();
        }
        [HttpGet("{id}")]
        public MusicCatalog Get(int id)
        {
            return _context.catalogMusics.Find(id);
        }  

        [HttpPost]
        public void Post([FromBody] MusicCatalog value)
        {
            _context.catalogMusics.Add(value);
            _context.SaveChanges();
        }

        [HttpGet("Images/{id}")]
        public MusicCatalog GetImage(int id)
        {
            return _context.catalogMusics.Find(id);
        }

        [HttpPost("Images")]
        public async Task<ActionResult> PostImage([FromForm] IFormFile file)
        {
            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                ms.Seek(0, SeekOrigin.Begin);
                var result = _blobClient.UploadBlob("test.jpg", ms);
                return Ok(result);
            }            
        }
    }
}
