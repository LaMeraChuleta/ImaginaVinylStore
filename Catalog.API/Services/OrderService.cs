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
            var options = new SessionGetOptions();
            options.AddExpand("line_items");
            var service = new SessionService();
            Session sessionWithLineItems = service.Get(session.Id, options);
            StripeList<LineItem> lineItems = sessionWithLineItems.LineItems;

            var priceStripeId = lineItems
                .Select(x => x.Price.ProductId)
                .ToArray();

            var musicCatalog = await _context.MusicCatalog
                .Where(x => priceStripeId.Contains(x.IdProductStripe))
                .ToListAsync();

            var order = new Order
            {
                ApplicationUserId = session.ClientReferenceId,
                CatalogMusics = musicCatalog,
            };

            await _context.Order.AddAsync(order);
            await _context.SaveChangesAsync();

            musicCatalog.ForEach(x =>
            {
                x.ActiveInStripe = false;
                x.Sold = true;
            });

            _context.MusicCatalog.UpdateRange(musicCatalog);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Order>> GetAsync(string idAspNetUser)
        {
            return await _context.Order
                .Where(x => x.ApplicationUserId == idAspNetUser)
                .Include(x => x.CatalogMusics)
                .Include(x => x.AudioCatalogs)
                .ToArrayAsync();
        }
    }
}
