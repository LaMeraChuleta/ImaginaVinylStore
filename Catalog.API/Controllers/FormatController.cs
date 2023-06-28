using Microsoft.AspNetCore.Authorization;
using SharedApp.Models;
using Microsoft.AspNetCore.Mvc;
using SharedApp.Data;

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
    public IEnumerable<Format> Get()
    {
        return _context.Formats.ToArray();
    }

    [HttpPost, Authorize]
    public Format Post([FromBody] Format value)
    {
        _context.Formats.Add(value);
        _context.SaveChanges();
        return value;
    }
}