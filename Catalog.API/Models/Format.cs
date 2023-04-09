using Microsoft.CodeAnalysis.CodeActions;

namespace Catalog.API.Models
{
    public class Format
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public IEnumerable<Presentation>? Presentations { get; set; }
    }
}
