using Stripe;
using Stripe.Checkout;
using System.Security.Claims;

namespace Catalog.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : Controller
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string endpointSecret = "whsec_2bacb0ffe54bdc9d94dd067b03cb0730c67ab567f143c5b76c5f8502ab5940b8";
    public OrderController(IHttpContextAccessor httpContextAccessor, AppDbContext context)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost]
    public async Task<IResult> Post()
    {
        try
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeEvent = EventUtility.ParseEvent(json, throwOnApiVersionMismatch: false);

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

                    var order = new Order
                    {
                        ApplicationUserId = session.ClientReferenceId,
                        CatalogMusics = musicCatalog,
                    };

                    await _context.Order.AddAsync(order);
                    await _context.SaveChangesAsync();

                    musicCatalog.ForEach(x =>
                    {
                        x.ActiveInStripe = false;
                        x.Sold = true;
                    });
                    _context.MusicCatalog.UpdateRange(musicCatalog);
                    await _context.SaveChangesAsync();
                }
            }
            return Results.Ok();
        }
        catch (Exception exeption)
        {
            Console.WriteLine(exeption.Message);
            return Results.BadRequest(exeption.Message);
        }
    }

    [HttpGet]
    [Authorize]
    public IResult Get()
    {
        var id = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var orders = _context.Order
            .Where(x => x.ApplicationUserId == id)
            .Include(x => x.CatalogMusics)
            .Include(x => x.AudioCatalogs)
            .ToArray();

        //if(orders.Any()) 
        return Results.Ok(orders);

        //return Results.NoContent();
    }
}