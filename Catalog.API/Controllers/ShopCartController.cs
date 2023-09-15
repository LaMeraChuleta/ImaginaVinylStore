using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedApp.Data;
using SharedApp.Models;
using Stripe.Checkout;
using System.Security.Claims;

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
    [HttpGet("Strapi")]
    public ActionResult PostTest()
    {
        var domain = "http://localhost:7285";
        var options = new SessionCreateOptions
        {
            LineItems = new List<SessionLineItemOptions>
                {
                  new SessionLineItemOptions
                  {
                    // Provide the exact Price ID (for example, pr_1234) of the product you want to sell
                    Price = "price_1NplPQF4FZD9gDsPuj6AdpwF",
                    Quantity = 1
                  },
                     new SessionLineItemOptions
                  {
                    // Provide the exact Price ID (for example, pr_1234) of the product you want to sell
                    Price = "price_1NplPQF4FZD9gDsPuj6AdpwF",
                    Quantity = 1
                  }
                },
            Mode = "payment",
            SuccessUrl = domain + "/success.html",
            CancelUrl = domain + "/cancel.html",
        };
        var service = new SessionService();
        Session session = service.Create(options);

        Response.Headers.Add("Location", session.Url);
        return Ok(session.Url);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IResult> Delete(int id)
    {
        if (!ModelState.IsValid) return Results.BadRequest();

        var shopCart = await _context.ShopCart.FindAsync(id);
        var existShopCart = shopCart is not null;
        if (existShopCart)
        {
            _context.ShopCart.Remove(shopCart!);
            _context.SaveChanges();
            return Results.Ok(true);
        }
        return Results.Ok(false);
    }
}