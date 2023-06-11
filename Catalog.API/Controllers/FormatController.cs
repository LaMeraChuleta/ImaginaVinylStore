using SharedApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedApp.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

    [HttpPost]
    public Format Post([FromBody] Format value)
    {
        _context.Formats.Add(value);
        _context.SaveChanges();
        return value;
    }
}