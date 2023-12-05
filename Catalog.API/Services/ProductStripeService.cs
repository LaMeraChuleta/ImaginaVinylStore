using Stripe;

namespace Catalog.API.Services
{
    public class ProductStripeService : IProductStripeService
    {
        private readonly AppDbContext _context;
        public ProductStripeService(AppDbContext context)
        {
            _context = context;
        }
        public bool Create(MusicCatalog value)
        {
            try
            {
                var name = $"{value.Title}-{value.Artist.Name}";

                var result = value.Images is not null
                    ? CreateInStripe(name, value.Price, value.Images.Select(x => x.Url).ToList())
                    : CreateInStripe(name, value.Price);

                value = _context.MusicCatalog.First(x => x.Id == value.Id);
                value.IdProductStripe = result.Id;
                value.IdPriceStripe = result.DefaultPriceId;
                value.ActiveInStripe = true;
                _context.MusicCatalog.Update(value);
                _context.SaveChanges();

                return true;
            }
            catch
            {
                throw;
            }
        }
        public bool Create(AudioCatalog value)
        {
            try
            {
                var name = $"{value.Name}-{value.Brand}";
                var result = value.Images is not null
                    ? CreateInStripe(name, value.Price, value.Images.Select(x => x.Url).ToList())
                    : CreateInStripe(name, value.Price);

                value = _context.AudioCatalog.First(x => x.Id == value.Id);
                value.IdProductStripe = result.Id;
                value.IdPriceStripe = result.DefaultPriceId;
                value.ActiveInStripe = true;
                _context.AudioCatalog.Update(value);
                _context.SaveChanges();

                return true;
            }
            catch
            {
                throw;
            }
        }
        private static Product CreateInStripe(string name, int price, List<string>? images = null)
        {
            if (images is not null)
            {
                images = images.Any() ? images : null;
            }

            var options = new ProductCreateOptions()
            {
                Name = name,
                Images = images,
                DefaultPriceData = new ProductDefaultPriceDataOptions()
                {
                    UnitAmount = price * 100,
                    Currency = "mxn"
                }
            };
            var service = new ProductService();
            return service.Create(options);
        }
    }
}
