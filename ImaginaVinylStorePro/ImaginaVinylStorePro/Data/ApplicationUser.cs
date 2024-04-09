using Microsoft.AspNetCore.Identity;
using SharedApp.Models;

namespace ImaginaVinylStorePro.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public virtual IEnumerable<Order> Orders { get; set; }
    }

}
