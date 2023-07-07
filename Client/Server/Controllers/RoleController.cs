using Client.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SharedApp.Data;
using SharedApp.Models;

namespace Client.Server.Controllers;


[Route("api/[controller]")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public RoleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }
    
    [HttpGet]
    [Authorize]
    public IEnumerable<IdentityRole> Get()
    {
        return _roleManager.Roles.ToArray();
    }

    [HttpPost]
    [Authorize]
    public async Task<IdentityRole?> Post(string roleName)
    {
        if ((await _roleManager.CreateAsync(new IdentityRole(roleName))).Succeeded)
        {
            return await _roleManager.FindByNameAsync(roleName);
        }

        return null;
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