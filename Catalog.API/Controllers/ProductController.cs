using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        //[Authorize]
        //public IResult Post([FromBody] MusicCatalog value)
        public IResult Post()
        {
            //if (!ModelState.IsValid) return Results.BadRequest();
            var value = _context.MusicCatalog
                .Include(x => x.Artist)
                .Include(x => x.Product)
                .FirstOrDefault(x => x.Id == 10)!;

            var images = _context.ImageCatalog
                .Where(x => x.MusicCatalogId == 10)
                .Select(x => x.Url)
                .ToList();

            var options = new ProductCreateOptions()
            {
                Name = $"{value.Title}-{value.Artist!.Name}",
                //Images = images,
                DefaultPriceData = new ProductDefaultPriceDataOptions()
                {
                    UnitAmount = value.Price,
                    Currency = "mxn"
                }
            };
            var service = new ProductService();
            var result = service.Create(options);

            var newProduct = new ProductCatalog()
            {
                IdProductStripe = result.Id,
                IdPriceStripe = result.DefaultPriceId,
                Name = options.Name
            };

            value.Product.Add(newProduct);
            _context.MusicCatalog.Update(value);
            _context.SaveChanges();

            return Results.Ok(value);
        }
    }
}
