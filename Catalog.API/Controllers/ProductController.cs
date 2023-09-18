using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedApp.Data;
using SharedApp.Models;
using Stripe;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {

        private readonly AppDbContext _context;
        //private readonly IHttpContextAccessor _httpContextAccessor;
        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize]
        public IResult Post([FromBody] MusicCatalog value)
        {
            if (!ModelState.IsValid) return Results.BadRequest();

            var images = _context.ImageCatalog
                .Where(x => x.MusicCatalogId == value.Id)
                .Select(x => x.Url)
                .ToList();

            var options = new ProductCreateOptions()
            {
                Name = $"{value.Title}-{value.Artist!.Name}",
                Images = images,
                DefaultPriceData = new ProductDefaultPriceDataOptions()
                {
                    UnitAmount = value.Price,
                    Currency = "mxn"
                }
            };
            var service = new ProductService();
            service.Create(options);

            _context.MusicCatalog.Add(value);
            _context.SaveChanges();
            return Results.Ok(value);
        }
    }
}
