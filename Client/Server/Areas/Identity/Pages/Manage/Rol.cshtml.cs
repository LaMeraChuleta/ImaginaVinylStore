using Client.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client.Server.Areas.Identity.Pages.Manage;

public class RolModel : PageModel
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public string Message { get; set; }
    
    public RolModel(
        RoleManager<IdentityRole> roleManager, 
        UserManager<ApplicationUser> userManager
    )
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }


    public Task OnGetAsync(string returnUrl = null)
    {
        Message = "HOLA MUNDO!!";
        return Task.CompletedTask;
    }
}