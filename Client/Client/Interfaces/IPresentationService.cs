namespace Client.App.Interfaces
{
    public interface IPresentationService
    {
        Task<List<Presentation>> GetAsync();
        Task<Presentation> CreateAsync(Presentation presentation);
    }
}
