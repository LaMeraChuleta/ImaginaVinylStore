using Client.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Client.Server.Controllers;

public class RolController : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    private RolModel ModelRol { get; }

    public RolController(
        RoleManager<IdentityRole> roleManager,
        UserManager<ApplicationUser> userManager
    )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        ModelRol = new RolModel(_userManager)
        {
            Users = _userManager.Users.ToList()
        };
    }

    [HttpGet]
    public IActionResult Index(string id = "", bool isOpenModal = false)
    {
        ModelRol.IsOpenAddRolToUser = isOpenModal;
        ModelRol.Id = id;
        return View(ModelRol);
    }

    [HttpPost]
    public async Task<IActionResult> AssignRolToUser(string rolName, string id = "")
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (user is not null)
        {
            if (!await _roleManager.RoleExistsAsync(rolName))
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(rolName));
                if (result.Succeeded) return View("Index", ModelRol);
            }
            await _userManager.AddToRoleAsync(user, rolName);
        }

        ModelRol.IsOpenAddRolToUser = false;
        return View("Index", ModelRol);
    }
}