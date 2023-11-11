using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedApp.Data;
using Stripe.Checkout;
using System.Security.Claims;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string domain = "https://localhost:7197/";

        public CheckoutController(IHttpContextAccessor httpContextAccessor, AppDbContext context)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody] Dictionary<string, string> value)
        {
            var id = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var idCatalogMusic = value
                .Select(x => Convert.ToInt32(x.Value))
                .ToList();

            var product = _context.MusicCatalog
                .Where(x => idCatalogMusic
                .Contains(x.Id))
                .Select(x => new SessionLineItemOptions()
                {
                    Price = x.IdPriceStripe,
                    Quantity = 1
                })
                .ToList();

            var options = new SessionCreateOptions
            {
                LineItems = product,
                ClientReferenceId = id,
                Mode = "payment",
                SuccessUrl = domain,
                CancelUrl = domain,
            };

            var service = new SessionService();
            Session session = service.Create(options);
            return Ok(session.Url);
        }
    }
}