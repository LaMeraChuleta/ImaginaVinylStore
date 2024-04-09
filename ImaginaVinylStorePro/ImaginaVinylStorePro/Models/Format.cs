using System.ComponentModel.DataAnnotations;

namespace SharedApp.Models;

public class Format
{
    public int Id { get; set; }
    [Required] public string? Name { get; set; }
    public IEnumerable<Presentation>? Presentations { get; set; }
}