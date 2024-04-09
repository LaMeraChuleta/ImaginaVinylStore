using System.ComponentModel.DataAnnotations.Schema;

namespace SharedApp.Models
{
    public class Order
    {
        // Primary key
        public int Id { get; set; }

        public string Name { get; set; }

        // Foreign key  
        [ForeignKey("ApplicationUser")]
        public string? ApplicationUserId { get; set; }
        public int ShippingAddressId { get; set; }

        // Navigation properties
        public virtual ShippingAddress ShippingAddress { get; set; }
        public virtual IEnumerable<MusicCatalog> CatalogMusics { get; set; }
        public virtual IEnumerable<AudioCatalog> AudioCatalogs { get; set; }
    }
}