using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedApp.Data;
using SharedApp.Models;

namespace Catalog.API.Controllers;

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
    public IResult Get()
    {
        return Results.Ok(_context.Presentations.ToArray());
    }

    [HttpPost]
    [Authorize]
    public IResult Post([FromBody] Presentation value)
    {
        if (!ModelState.IsValid) return Results.BadRequest();

        _context.Presentations.Add(value);
        _context.SaveChanges();
        return Results.Ok(value);
    }
}