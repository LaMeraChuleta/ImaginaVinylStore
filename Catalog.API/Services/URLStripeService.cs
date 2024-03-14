namespace Catalog.API.Services
{
    public class URLStripeService : IURLStripeService
    {
        private const string domain = "https://localhost:7197/";
        public URLStripeService()
        {
        }
        public string CreateURLCheckoutPayment(string idAspNetUser, string email, List<ShopCartWrapper> items)
        {
            try
            {
                var products = items
                    .ConvertAll(x => new SessionLineItemOptions()
                    {
                        Price = x.GetIdPriceStripe(),
                        Quantity = 1
                    })
;
                var options = new SessionCreateOptions
                {
                    LineItems = products,                    
                    ClientReferenceId = idAspNetUser,                    
                    CustomerEmail = email,
                    ShippingAddressCollection = new SessionShippingAddressCollectionOptions
                    {
                        AllowedCountries = new List<string> { "MX" },

                    },
                    Locale = "es",
                    Mode = "payment",
                    SuccessUrl = domain + "Checkout/Complete",
                    CancelUrl = domain + "CartSummary",
                };

                var service = new SessionService();
                Session session = service.Create(options);
                return session.Url;
            }
            catch
            {
                throw;
            }
        }
    }
}
