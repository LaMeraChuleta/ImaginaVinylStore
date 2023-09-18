using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeEvent = EventUtility.ParseEvent(json);

            if (stripeEvent.Type == Events.CheckoutSessionCompleted)
            {
                var r = stripeEvent.Data.Object as Stripe.Checkout.Session;

                foreach (var item in r.LineItems)
                {
                    Console.WriteLine(item.ToString());
                }
            }
            return Ok();
        }
    }
}