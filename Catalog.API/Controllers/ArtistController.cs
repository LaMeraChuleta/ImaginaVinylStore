using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedApp.Data;
using SharedApp.Models;

namespace Catalog.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ArtistController : ControllerBase
{
    private readonly BlobContainerClient _blobClient;
    private readonly AppDbContext _context;

    public ArtistController(AppDbContext context, IConfiguration config)
    {        
        _context = context;
        _blobClient = new BlobContainerClient(config["BlobConnectionString"], config["BlobContainerName"]);
    }

    [HttpGet]
    public IResult Get()
    {
        return Results.Ok(_context.Artist
            .Include(x => x.Image)
            .ToArray());
    }

    [HttpPost]
    [Authorize]
    public IResult Post([FromBody] Artist value)
    {
        if (!ModelState.IsValid) return Results.BadRequest();

        _context.Artist.Add(value);
        _context.SaveChanges();
        return Results.Ok(value);
    }

    [HttpGet("Images")]
    public IResult GetImage(int id)
    {
        return Results.Ok(_context.ImageCatalog.Find(id));
    }

    [HttpPost("Images")]
    [Authorize]
    public async Task<IResult> PostImage(List<IFormFile> file, int id)
    {
        using var ms = new MemoryStream();
        var newImageArtist = new ImageArtist
        {
            ArtistId = id,
            Name = file.FirstOrDefault()?.FileName.Split(".")[0]
                   + new Random().NextInt64() + "."
                   + file.FirstOrDefault()?.FileName.Split(".")[1]
        };
        newImageArtist.Url = $"https://storageimagina.blob.core.windows.net/img/{newImageArtist.Name}";

        await file.FirstOrDefault()?.CopyToAsync(ms)!;
        ms.Seek(0, SeekOrigin.Begin);
        await _blobClient.UploadBlobAsync(newImageArtist.Name, ms);
        await _context.ImageArtist.AddAsync(newImageArtist);
        await _context.SaveChangesAsync();
        return Results.Ok(newImageArtist);
    }
}