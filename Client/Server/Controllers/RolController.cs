using Client.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SharedApp.Data;
using SharedApp.Models;

namespace Client.Server.Controllers;


[Route("api/[controller]")]
[ApiController]
public class RolController : ControllerBase
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public RolController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }
    
    [HttpGet]
    [Authorize]
    public IEnumerable<string> Get()
    {
        return _roleManager.Roles.Select(x => x.Name).ToArray();
    }

    [HttpPost]
    [Authorize]
    public async Task<bool> Post(string roleName)
    {
        return (await _roleManager.CreateAsync(new IdentityRole(roleName))).Succeeded;
    }

    [HttpPost("AddRolUser")]
    [Authorize]
    public async Task<bool> PostAddRoleUser(string roleName)
    {
        var user = await _userManager.GetUserAsync(User);
        var result = await _userManager.AddToRoleAsync(user, roleName);
        return result.Succeeded;
    } 
}