namespace Client.App.Interfaces
{
    public interface IFormatService
    {
        Task<List<Format>> GetAsync();
        Task<Format> CreateAsync(Format format);
    }
}
