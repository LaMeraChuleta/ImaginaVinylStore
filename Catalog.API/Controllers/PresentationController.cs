using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
    public IEnumerable<Presentation> Get()
    {
        return _context.Presentations.ToArray();
    }

    [HttpPost]
    [Authorize]
    public Presentation? Post([FromBody] Presentation value)
    {
        if (!ModelState.IsValid) return null;
        
        _context.Presentations.Add(value);
        _context.SaveChanges();
        return value;
    }
}