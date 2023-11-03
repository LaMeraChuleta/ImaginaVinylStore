using System.ComponentModel.DataAnnotations.Schema;

namespace SharedApp.Models
{
    public class Orders
    {
        public int Id { get; set; }
        // Foreign key  
        [ForeignKey("ApplicationUser")]
        public string? ApplicationUserId { get; set; }
        public virtual IEnumerable<MusicCatalog> CatalogMusics { get; set; }
        public virtual IEnumerable<AudioCatalog> AudioCatalogs { get; set; }
    }
}
