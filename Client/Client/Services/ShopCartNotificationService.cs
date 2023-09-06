﻿using Client.App.Interfaces;
using static Client.App.Shared.NavBar;

namespace Client.App.Services
{
    public class ShopCartNotificationService : IShopCartNotificationService
    {        
        public delegate void ShopCartCountUpdate(int shopCartCount);
        public static event ShopCartCountUpdate OnShopCartCountUpdate;

        public void NotifitShopCartCountChanges(int shopCartCount)
        {
            OnShopCartCountUpdate?.Invoke(shopCartCount);
        }
    }
}
