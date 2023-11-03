using Microsoft.AspNetCore.Identity;
using SharedApp.Models;

namespace Client.Server.Models;

public class ApplicationUser : IdentityUser
{
    public virtual Orders Orders { get; set; }
}