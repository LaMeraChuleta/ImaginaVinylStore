using Microsoft.AspNetCore.Components.Forms;
using SharedApp.Models;

namespace Client.App.Interfaces
{
    public interface IAudioCatalogService
    {
        Task<List<AudioCatalog>> GetAsync();
        Task<AudioCatalog> GetByIdAsync(int id);
        Task<AudioCatalog> CreateAsync(AudioCatalog audioCatalog);
        Task<ImageAudio> CreateImageAsync(int idAudioCatalog, IBrowserFile file);
        Task<bool> UpdateAsync(AudioCatalog audioCatalog);
    }
}
