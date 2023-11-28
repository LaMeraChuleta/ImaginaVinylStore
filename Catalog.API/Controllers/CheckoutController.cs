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
            var email = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Email)!;

            var music = value.Where(x => x.Key.Split("-")[0] == "music").ToList();
            var audio = value.Where(x => x.Key.Split("-")[0] == "audio").ToList();

            var idCatalogMusic = music
                .Select(x => Convert.ToInt32(x.Value))
                .ToList();

            var idAudioMusic = audio
                .Select(x => Convert.ToInt32(x.Value))
                .ToList();

            var productAudio = _context.AudioCatalog
                .Where(x => idAudioMusic
                .Contains(x.Id))
                .Select(x => new SessionLineItemOptions()
                {
                    Price = x.IdPriceStripe,
                    Quantity = 1
                })
                .ToList();

            var productMusic = _context.MusicCatalog
                .Where(x => idCatalogMusic
                .Contains(x.Id))
                .Select(x => new SessionLineItemOptions()
                {
                    Price = x.IdPriceStripe,
                    Quantity = 1
                })
                .ToList();

            var products = productMusic.Concat(productAudio).ToList();

            var options = new SessionCreateOptions
            {
                LineItems = products,
                ClientReferenceId = id,
                CustomerEmail = email,
                ShippingAddressCollection = new Stripe.Checkout.SessionShippingAddressCollectionOptions
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