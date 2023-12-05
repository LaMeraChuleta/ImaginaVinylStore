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
        return Results.Ok(_context.Presentation.ToArray());
    }

    [HttpPost]
    [Authorize]
    public IResult Post([FromBody] Presentation value)
    {
        if (!ModelState.IsValid) return Results.BadRequest();

        _context.Presentation.Add(value);
        _context.SaveChanges();
        return Results.Ok(value);
    }
}