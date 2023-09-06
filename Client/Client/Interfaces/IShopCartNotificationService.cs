namespace Client.App.Interfaces
{
    public interface IShopCartNotificationService
    {
        delegate void ShopCartCountUpdate(int shopCartCount);
        static event ShopCartCountUpdate OnShopCartCountUpdate;
        void NotifitShopCartCountChanges(int shopCartCount);
    }
}
