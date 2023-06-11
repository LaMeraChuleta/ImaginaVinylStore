using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedApp.Data;
using SharedApp.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
    public Genre Post([FromBody] Genre value)
    {
        _context.Genres.Add(value);
        _context.SaveChanges();
        return value;
    }
}