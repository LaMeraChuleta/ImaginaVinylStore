using Microsoft.AspNetCore.Identity;

namespace Client.Server.Models
{
    public class RolModel
    {
        public RolModel(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public string Id { get; set; }
        public string RolName { get; set; }
        public bool IsOpenAddRolToUser { get; set; }
        public List<ApplicationUser> Users { get; set; }
        private UserManager<ApplicationUser> UserManager { get; }

        public async Task<List<string>> GetRolesAsync(ApplicationUser user)
        {
            var roles = await UserManager.GetRolesAsync(user);
            return roles.Any() ? roles.ToList() : new List<string> { "Sin Roles" };
        }
    }
}
