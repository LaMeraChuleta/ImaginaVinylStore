namespace Catalog.API.Interfaces
{
    public interface IOrderService
    {
        Task<bool> CreateAsync(Session session);
        Task<IEnumerable<Order>> GetAsync(string idAspNetUser);
    }
}