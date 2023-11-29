using Catalog.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedApp.Models;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductStripeService _productStripeService;

        public ProductController(IProductStripeService productStripeService)
        {
            _productStripeService = productStripeService;
        }

        [HttpPost("MusicCatalog")]
        [Authorize]
        public IResult Post([FromBody] MusicCatalog value)
        {
            if (!ModelState.IsValid) return Results.BadRequest();

            _productStripeService.Create(value);
            return Results.Ok(value);
        }

        [HttpPost("AudioCatalog")]
        [Authorize]
        public IResult Post([FromBody] AudioCatalog value)
        {
            if (!ModelState.IsValid) return Results.BadRequest();

            _productStripeService.Create(value);
            return Results.Ok(value);
        }
    }
}
