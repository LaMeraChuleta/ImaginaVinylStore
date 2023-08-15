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
    public IResult Get()
    {
        return Results.Ok(_context.Genres.ToArray());
    }

    [HttpGet("{id}")]
    public IResult Get(int id)
    {
        return Results.Ok(_context.Genres.Find(id));
    }

    [HttpPost]
    [Authorize]
    public IResult Post([FromBody] Genre value)
    {
        if (!ModelState.IsValid) return Results.BadRequest();

        _context.Genres.Add(value);
        _context.SaveChanges();
        return Results.Ok(value);
    }
}