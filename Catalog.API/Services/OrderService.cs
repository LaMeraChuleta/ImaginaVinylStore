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

            var order = new Order()
            {
                ApplicationUserId = session.ClientReferenceId,
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
                .Include(x => x.CatalogMusics)
                .Include(x => x.AudioCatalogs)
                .ToArrayAsync();
        }
        private static IEnumerable<string> GetLineItemStripe(Session session)
        {
            var options = new SessionGetOptions();
            options.AddExpand("line_items");
            var service = new SessionService();
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
