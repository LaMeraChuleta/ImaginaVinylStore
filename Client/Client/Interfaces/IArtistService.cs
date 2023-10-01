using Microsoft.AspNetCore.Components.Forms;
using SharedApp.Models;

namespace Client.App.Interfaces
{
    public interface IArtistService
    {
        Task<List<Artist>> GetAsync();
        Task<Artist> CreateAsync(Artist artist);
        Task<ImageArtist> CreateImageAsync(Artist artist, IBrowserFile file);
    }
}
