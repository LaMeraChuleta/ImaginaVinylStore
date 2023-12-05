namespace Catalog.API.Interfaces
{
    public interface IProductStripeService
    {
        bool Create(MusicCatalog value);
        bool Create(AudioCatalog value);
    }
}
