namespace Catalog.API.Interfaces
{
    public interface IURLStripeService
    {
        string CreateURLCheckoutPayment(string idAspNetUser, string email, List<ShopCartWrapper> items);
    }
}