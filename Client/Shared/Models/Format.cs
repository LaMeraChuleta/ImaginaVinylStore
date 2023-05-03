namespace SharedApp.Models
{
    public class Format
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public IEnumerable<Presentation>? Presentations { get; set; }
    }
}
