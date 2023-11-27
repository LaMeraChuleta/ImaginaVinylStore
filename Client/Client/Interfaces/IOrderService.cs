﻿using SharedApp.Models;

namespace Client.App.Interfaces
{
    public interface IOrderService
    {
        Task<List<Order>> GetAsync();
    }
}
