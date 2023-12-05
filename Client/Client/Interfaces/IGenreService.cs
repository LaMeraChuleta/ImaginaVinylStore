namespace Client.App.Interfaces
{
    public interface IGenreService
    {
        Task<List<Genre>> GetAsync();
        Task<Genre> CreateAsync(Genre genre);
    }
}