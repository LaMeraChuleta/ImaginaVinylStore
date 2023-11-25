using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedApp.Data;
using SharedApp.Models;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OrdersController(IHttpContextAccessor httpContextAccessor, AppDbContext context)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public async Task Post()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeEvent = EventUtility.ParseEvent(json);

            if (stripeEvent.Type == Events.CheckoutSessionCompleted)
            {
                var session = stripeEvent.Data.Object as Session;
                if (session is not null)
                {
                    var options = new SessionGetOptions();
                    options.AddExpand("line_items");
                    var service = new SessionService();
                    Session sessionWithLineItems = service.Get(session.Id, options);
                    StripeList<LineItem> lineItems = sessionWithLineItems.LineItems;

                    var priceStripeId = lineItems
                        .Select(x => x.Price.ProductId)
                        .ToArray();

                    var musicCatalog = await _context.MusicCatalog
                        .Where(x => priceStripeId.Contains(x.IdProductStripe))
                        .ToListAsync();

                    var order = new Orders
                    {
                        ApplicationUserId = session.ClientReferenceId,
                        CatalogMusics = musicCatalog,
                    };
								
                    await _context.Orders.AddAsync(order);
                    musicCatalog.ForEach(x => {
                        x.ActiveInStripe = false;
                        x.Sold = true;
                    });
                    _context.MusicCatalog.UpdateRange(musicCatalog);
                    await _context.SaveChangesAsync();
                }
            }
        }

        [HttpGet]      
        public IResult Get()
        { 
            var service = new ProductService();
            var option = new ProductUpdateOptions { Active = true };

            service.Update("prod_OxBwfB3kPPgmSq", option);

            var id = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            return Results.Ok(_context.Orders
           .Where(x => x.ApplicationUserId == id)
           .Include(x => x.CatalogMusics)
           .ToArray());
        }
    }
}
