using Microsoft.Extensions.Hosting;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutController : Controller
    {
        private readonly IURLStripeService _urlStripeService;        
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CheckoutController(IHttpContextAccessor httpContextAccessor, IURLStripeService urlStripeService)
        {
            _urlStripeService = urlStripeService;            
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody] List<ShopCartWrapper> value)
        {
            var id = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var email = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Email)!;
            var url = _urlStripeService.CreateURLCheckoutPayment(id, email, value);
            return Ok(url);
        }
    }
}