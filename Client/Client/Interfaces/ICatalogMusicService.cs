using static Client.App.Services.CatalogMusicService;

namespace Client.App.Interfaces
{
    public interface ICatalogMusicService
    {
        Task<List<MusicCatalog>> GetAsync();
        Task<MusicCatalog> GetByIdAsync(int id);
        Task<List<MusicCatalog>> GetAsync(FilterForCatalogMusic filter);
        Task<MusicCatalog> CreateAsync(MusicCatalog musicCatalog);
        Task<ImageCatalog> CreateImageAsync(int idMusicCatalog, IBrowserFile file);
        Task<bool> UpdateAsync(MusicCatalog catalog);
        Task<bool> DeleteAsync(MusicCatalog catalog);
    }
}
