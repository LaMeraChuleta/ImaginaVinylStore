using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedApp.Data;
using Stripe.Checkout;
using System.Security.Claims;
using SharedApp.Extension;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CheckoutController(IHttpContextAccessor httpContextAccessor, AppDbContext context)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody]ShopCart shopCart)
        {
            var id = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var product = _context.MusicCatalog
                .Where(x => shopCart.GetItemsId().Contains(x.Id))
                .Select(x => new SessionLineItemOptions()
                {
                    Price = x.IdPriceStripe,
                    Quantity = 1
                })
                .ToList();

            const string domain = "http://localhost:7285";
            var options = new SessionCreateOptions
            {
                LineItems = product,
                ClientReferenceId = id,
                Mode = "payment",
                SuccessUrl = domain,
                CancelUrl = domain + "/ShopCart",
            };

            var service = new SessionService();
            Session session = service.Create(options);
            Response.Headers.Add("Location", session.Url);
            return Ok(session.Url);
        }
    }
}
