using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Security.Claims;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CheckoutController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [Authorize]
        public IActionResult Post()
        {

            var id = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

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
                    Price = "price_1NplPQF4FZD9gDsPuj6AdpwF",
                    Quantity = 1
                  }
                },
                ClientReferenceId = id,
                //Metadata = new Dictionary<string, string>()
                //{
                //    { "idUser", id }
                //},
                Mode = "payment",
                SuccessUrl = domain + "/success.html",
                CancelUrl = domain + "/cancel.html",
            };
            var service = new SessionService();
            Session session = service.Create(options);
            Response.Headers.Add("Location", session.Url);
            return Ok(session.Url);
        }
    }
}
