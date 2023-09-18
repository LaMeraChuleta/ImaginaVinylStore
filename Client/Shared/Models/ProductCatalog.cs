using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedApp.Models
{
    public class ProductCatalog
    {
        public int Id { get; set; }
        public string IdProductStripe { get; set; }
        public string IdPriceStripe { get; set; }
        public string Name { get; set; }
        [NotMapped]
        public virtual ICollection<MusicCatalog> MusicCatalogs { get; set; }
        [NotMapped]

        public virtual ICollection<AudioCatalog> AudioCatalogs{ get; set; }
    }
}
