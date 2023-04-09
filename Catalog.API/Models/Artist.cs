using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Hosting;

namespace Catalog.API.Models
{
    public class Artist
    {        
        public int Id { get; set; }
        public string? Name { get; set; }
		public ICollection<CatalogMusic> CatalogMusics { get; } = new List<CatalogMusic>();
	}
}
