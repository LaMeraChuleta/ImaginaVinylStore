using Microsoft.AspNetCore.Identity;
using SharedApp.Models;

namespace Client.Server.Models;

public class ApplicationUser : IdentityUser
{
    public virtual IEnumerable<Orders> Orders { get; set; }
}