﻿using System.ComponentModel.DataAnnotations;
using SharedApp.Validation;

namespace SharedApp.Models;

public class Presentation
{
    public int Id { get; set; }
    [Required] public string? Name { get; set; }
    [NotZero] public int FormatId { get; set; }
    public ICollection<MusicCatalog> CatalogMusics { get; } = new List<MusicCatalog>();
}