using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedApp.Data;
using SharedApp.Models;

namespace Catalog.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShopCartController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ShopCartController(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    public IResult Get()
    {
        var id = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        return Results.Ok(_context.ShopCart.Where(x => x.ApplicationUserId == id).ToArray());
    }

    [HttpPost]
    [Authorize]
    public IResult Post([FromBody] ShopCart value)
    {
        if (!ModelState.IsValid) return Results.BadRequest();
        
        value.ApplicationUserId = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        _context.ShopCart.Add(value);
        _context.SaveChanges();
        return Results.Ok(value);
    }
}