using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedApp.Data;
using SharedApp.Models;

namespace Catalog.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MusicCatalogController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly BlobContainerClient _blobClient;

    public MusicCatalogController(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _blobClient = new BlobContainerClient(config["BlobConnectionString"], config["BlobContainerName"]);
    }

    [HttpGet]
    public IEnumerable<MusicCatalog> Get()
    {
        return _context.MusicCatalogs
            .Include(x => x.Artist)
            .Include(x => x.Genre)
            .Include(x => x.Presentation)
            .Include(x => x.Format)
            .Include(x => x.Images)
            .ToArray();
    }

    [HttpGet("ById")]
    public ActionResult GetById(int id)
    {
        return Ok(_context.MusicCatalogs.Find(id));
    }

    [HttpPost]
    [Authorize]
    public MusicCatalog Post([FromBody] MusicCatalog value)
    {
        if (ModelState.IsValid) return null;
        
        _context.MusicCatalogs.Add(value);
        _context.SaveChanges();
        return value;

    }

    [HttpGet("Images")]
    public ActionResult GetImage(int id)
    {
        return Ok(_context.ImagesCatalog.Find(id));
    }

    [HttpPost("Images")]
    [Authorize]
    public async Task<ImageCatalog> PostImage(List<IFormFile> file, int id)
    {
        using var ms = new MemoryStream();
        var newImageCatalog = new ImageCatalog
        {
            MusicCatalogId = id,
            Name = file.FirstOrDefault()?.FileName.Split(".")[0]
                   + new Random().NextInt64() + "."
                   + file.FirstOrDefault()?.FileName.Split(".")[1]
        };
        newImageCatalog.Url = $"https://storageimagina.blob.core.windows.net/img/{newImageCatalog.Name}";

        await file.FirstOrDefault()?.CopyToAsync(ms)!;
        ms.Seek(0, SeekOrigin.Begin);
        await _blobClient.UploadBlobAsync(newImageCatalog.Name, ms);
        await _context.ImagesCatalog.AddAsync(newImageCatalog);
        await _context.SaveChangesAsync();
        return newImageCatalog;
    }
}