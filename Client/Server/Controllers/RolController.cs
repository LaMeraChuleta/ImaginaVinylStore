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
    // GET
    [HttpGet]
    public IActionResult Index(string id = "", bool isOpenModal = false)
    {
        ModelRol.IsOpenAddRolToUser = isOpenModal;
        ModelRol.Id = id;
        return View(ModelRol);
    }

    [HttpPost]
    public async Task<IActionResult> AssignRolToUser(string id = "")
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (user is not null)
        {
            if (!await _roleManager.RoleExistsAsync(ModelRol.RolName))
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(ModelRol.RolName));
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, ModelRol.RolName);
                }
            }
        }

        ModelRol.Id = string.Empty;
        ModelRol.RolName = string.Empty;
        ModelRol.IsOpenAddRolToUser = false;
        return View("Index", ModelRol);
    }
    
    public class RolModel
    {
        public string Id { get; set; }
        public string RolName { get; set; }
        public bool IsOpenAddRolToUser { get; set; }
        public List<ApplicationUser> Users { get; set; }
        private UserManager<ApplicationUser> UserManager { get; }
        public RolModel(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }
        public async Task<List<string>> GetRolesAsync(ApplicationUser user)
        {
            var roles = await UserManager.GetRolesAsync(user);
            return roles.Any() ? roles.ToList() : new List<string>() { "Sin Roles" };
        }
    }
}