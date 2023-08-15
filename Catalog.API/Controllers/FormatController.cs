using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedApp.Data;
using SharedApp.Models;

namespace Catalog.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FormatController : ControllerBase
{
    private readonly AppDbContext _context;

    public FormatController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IResult Get()
    {
        return Results.Ok(_context.Formats.ToArray());
    }

    [HttpPost]
    [Authorize]
    public IResult Post([FromBody] Format value)
    {
        if (!ModelState.IsValid) return Results.BadRequest();

        _context.Formats.Add(value);
        _context.SaveChanges();
        return Results.Ok(value);
    }
}