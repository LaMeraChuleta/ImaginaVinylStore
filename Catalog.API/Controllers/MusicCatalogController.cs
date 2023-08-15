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
    private readonly BlobContainerClient _blobClient;
    private readonly AppDbContext _context;

    public MusicCatalogController(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _blobClient = new BlobContainerClient(config["BlobConnectionString"], config["BlobContainerName"]);
    }

    [HttpGet]
    public IResult Get()
    {
        return Results.Ok(_context.MusicCatalogs
            .Include(x => x.Artist)
            .Include(x => x.Genre)
            .Include(x => x.Presentation)
            .Include(x => x.Format)
            .Include(x => x.Images)
            .ToArray());
    }

    [HttpGet("ById")]
    public IResult GetById(int id)
    {
        return Results.Ok(_context.MusicCatalogs
            .Include(x => x.Artist)
            .Include(x => x.Genre)
            .Include(x => x.Presentation)
            .Include(x => x.Format)
            .Include(x => x.Images)
            .First(x => x.Id == id));
    }

    [HttpGet("ForFilter")]
    public IResult GetByFilter(string? title, int? idGenre, int? idArtist, int? idFormat, int? idPresentation)
    {
        var data = _context.MusicCatalogs
            .Include(x => x.Artist)
            .Include(x => x.Genre)
            .Include(x => x.Presentation)
            .Include(x => x.Format)
            .Include(x => x.Images)
            .Where(x =>
                (title == null || x.Title.Contains(title)) &&
                (idGenre == null || x.Genre!.Id == idGenre) &&
                (idArtist == null || x.Artist!.Id == idArtist) &&
                (idPresentation == null || x.Presentation!.Id == idPresentation) &&
                (idFormat == null || x.Format!.Id == idFormat)
            )
            .ToArray();

        return Results.Ok(data);
    }

    [HttpGet("ForSearch")]
    public IResult GetForSearchBar(string querySearch)
    {
        return Results.Ok(_context.MusicCatalogs
            .Include(x => x.Artist)
            .Include(x => x.Genre)
            .Include(x => x.Presentation)
            .Include(x => x.Format)
            .Include(x => x.Images)
            .Where(x => x.Title.Contains(querySearch))
            .ToArray());
    }

    [HttpPost]
    [Authorize]
    public IResult Post([FromBody] MusicCatalog value)
    {
        if (!ModelState.IsValid) return Results.BadRequest();

        _context.MusicCatalogs.Add(value);
        _context.SaveChanges();
        return Results.Ok(value);
    }

    [HttpGet("Images")]
    public IResult GetImage(int id)
    {
        return Results.Ok(_context.ImagesCatalog.Find(id));
    }

    [HttpPost("Images")]
    [Authorize]
    public async Task<IResult> PostImage(List<IFormFile> file, int id)
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
        return Results.Ok(newImageCatalog);
    }
}