namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AudioCatalogController : Controller
    {
        private readonly BlobContainerClient _blobClient;
        private readonly AppDbContext _context;
        public AudioCatalogController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _blobClient = new BlobContainerClient(config["BlobConnectionString"], config["BlobContainerName"]);
        }

        [HttpGet]
        public IResult Get()
        {
            return Results.Ok(_context.AudioCatalog
                .Where(x => x.Sold == false)
                .Include(x => x.Images)
                .ToArray());
        }

        [HttpGet("{id}")]
        public IResult GetById(int id)
        {
            return Results.Ok(_context.AudioCatalog
                .Where(x => x.Sold!)
                .Include(x => x.Images)
                .First(x => x.Id == id));
        }

        [HttpPost]
        [Authorize]
        public IResult Post([FromBody] AudioCatalog value)
        {
            if (!ModelState.IsValid) return Results.BadRequest();

            _context.AudioCatalog.Add(value);
            _context.SaveChanges();
            return Results.Ok(value);
        }

        [HttpPut("{id}")]
        [Authorize]
        public IResult Put(int id, [FromBody] AudioCatalog value)
        {
            if (!ModelState.IsValid) return Results.BadRequest();

            _context.Entry(value).State = EntityState.Modified;
            return Results.Ok(Convert.ToBoolean(_context.SaveChanges()));
        }

        [HttpPost("Images")]
        [Authorize]
        public async Task<IResult> PostImage(List<IFormFile> file, int id)
        {
            using var ms = new MemoryStream();
            var newImageAudio = new ImageAudio
            {
                AudioCatalogId = id,
                Name = file.FirstOrDefault()?.FileName.Split(".")[0]
                       + new Random().NextInt64() + "."
                       + file.FirstOrDefault()?.FileName.Split(".")[1]
            };
            newImageAudio.Url = $"https://storageimagina.blob.core.windows.net/img/{newImageAudio.Name}";

            await file.FirstOrDefault()?.CopyToAsync(ms)!;
            ms.Seek(0, SeekOrigin.Begin);
            await _blobClient.UploadBlobAsync(newImageAudio.Name, ms);
            await _context.ImageAudio.AddAsync(newImageAudio);
            await _context.SaveChangesAsync();
            return Results.Ok(newImageAudio);
        }
    }
}
