using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedApp.Data;
using SharedApp.Models;

namespace Catalog.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GenreController : ControllerBase
{
    private readonly AppDbContext _context;

    public GenreController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IEnumerable<Genre> Get()
    {
        return _context.Genres.ToArray();
    }

    [HttpGet("{id}")]
    public Genre? Get(int id)
    {
        return _context.Genres.Find(id);
    }

    [HttpPost]
    [Authorize]
    public Genre Post([FromBody] Genre value)
    {
        if (!ModelState.IsValid) return null;
        
        _context.Genres.Add(value);
        _context.SaveChanges();
        return value;
    }
}