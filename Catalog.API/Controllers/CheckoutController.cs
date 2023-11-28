using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedApp.Extension;
using Stripe.Checkout;
using System.Security.Claims;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string domain = "https://localhost:7197/";

        public CheckoutController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody] List<ShopCartWrapper> value)
        {
            var id = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var email = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Email)!;

            var products = value
                .Select(x => new SessionLineItemOptions()
                {
                    Price = x.GetIdPriceStripe(),
                    Quantity = 1
                })
                .ToList();

            var options = new SessionCreateOptions
            {
                LineItems = products,
                ClientReferenceId = id,
                CustomerEmail = email,
                ShippingAddressCollection = new SessionShippingAddressCollectionOptions
                {
                    AllowedCountries = new List<string> { "MX" }
                },
                Mode = "payment",
                SuccessUrl = domain + "Checkout/Complete",
                CancelUrl = domain + "CartSummary",
            };

            var service = new SessionService();
            Session session = service.Create(options);
            return Ok(session.Url);
        }
    }
}