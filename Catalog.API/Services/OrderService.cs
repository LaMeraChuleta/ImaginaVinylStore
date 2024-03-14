namespace Catalog.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;
        public OrderService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateAsync(Session session)
        {
            var priceStripeId = GetLineItemStripe(session);

            var musicCatalog = await ChangeStateToSold<MusicCatalog>(priceStripeId);
            var audioCatalog = await ChangeStateToSold<AudioCatalog>(priceStripeId);

            var shippingAddress = await CreateShippingAddress(session);

            var order = new Order()
            {
                Name = session.ShippingDetails.Name,
                ApplicationUserId = session.ClientReferenceId,
                ShippingAddress = shippingAddress,
                CatalogMusics = musicCatalog,
                AudioCatalogs = audioCatalog
            };

            await _context.Order.AddAsync(order);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<IEnumerable<Order>> GetAsync(string idAspNetUser)
        {
            return await _context.Order
                .Where(x => x.ApplicationUserId == idAspNetUser)
                .Include(x => x.ShippingAddress)
                .Include(x => x.CatalogMusics)
                    .ThenInclude(x => x.Format)
                .Include(x => x.CatalogMusics)
                    .ThenInclude(x => x.Images)
                .Include(x => x.AudioCatalogs)
                    .ThenInclude(x => x.Images)
                .ToArrayAsync();
        }
        private async Task<ShippingAddress> CreateShippingAddress(Session session)
        {
            var shippingAddres = new ShippingAddress
            {
                StreetAddres1 = session.ShippingDetails.Address.Line1,
                StreetAddress2 = session.ShippingDetails.Address.Line2,
                Country = session.ShippingDetails.Address.Country,
                City = session.ShippingDetails.Address.City,
                Phone = "55 44 10 53 55",
                PostalCode = session.ShippingDetails.Address.PostalCode
            };

            await _context.ShippingAddress.AddAsync(shippingAddres);
            await _context.SaveChangesAsync();

            return shippingAddres;
        }
        private static IEnumerable<string> GetLineItemStripe(Session session)
        {
            var service = new SessionService();
            var options = new SessionGetOptions();
            options.AddExpand("line_items");

            Session sessionWithLineItems = service.Get(session.Id, options);
            StripeList<LineItem> lineItems = sessionWithLineItems.LineItems;

            return lineItems
                .Select(x => x.Price.ProductId)
                .ToArray();
        }
        private async Task<List<T>> ChangeStateToSold<T>(IEnumerable<string> priceStripeId)
            where T : class, IStripeCatalogBase
        {
            var catalog = await _context.Set<T>()
                .Where(x => priceStripeId.Contains(x.IdProductStripe))
                .ToListAsync();

            catalog.ForEach(x =>
            {
                x.ActiveInStripe = false;
                x.Sold = true;
            });

            _context.Set<T>().UpdateRange(catalog);
            await _context.SaveChangesAsync();
            return catalog;
        }
    }
}
