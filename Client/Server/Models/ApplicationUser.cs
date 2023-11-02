using SharedApp.Models;
using Microsoft.AspNetCore.Identity;

namespace Client.Server.Models;

public class ApplicationUser : IdentityUser
{    
    public virtual Orders Orders { get; set; }
}