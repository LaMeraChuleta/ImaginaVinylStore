using Microsoft.AspNetCore.Components.Forms;
using SharedApp.Models;

namespace Client.App.Interfaces
{
    public interface IAudioCatalogService
    {
        Task<List<AudioCatalog>> GetAsync();
        Task<AudioCatalog> CreateAsync(AudioCatalog audioCatalog);
        Task<ImageAudio> CreateImageAsync(AudioCatalog audioCatalog, IBrowserFile file);
    }
}
