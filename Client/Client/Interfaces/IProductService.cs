﻿using SharedApp.Models;

namespace Client.App.Interfaces
{
    public interface IProductService
    {
        Task<MusicCatalog> CreateAsync(MusicCatalog musicCatalog);
    }
}