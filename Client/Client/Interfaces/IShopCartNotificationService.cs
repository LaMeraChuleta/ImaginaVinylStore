namespace Client.App.Interfaces
{
    public interface IShopCartNotificationService
    {
        delegate void ShopCartCountUpdate(int shopCartCount);
        void NotifitShopCartCountChanges(int shopCartCount);
    }
}
