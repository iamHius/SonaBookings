using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SonaBookings.Areas.Admin.ViewModels;
using SonaBookings.Areas.Identity.Data;

namespace SonaBookings.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountManagerController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountManagerController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName,
                };
                IdentityResult result = await _roleManager.CreateAsync(identityRole);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRole","AccountManager", new { area = "Admin" });
                }
                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            
            return View(model);
        }

        public IActionResult ListRole()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        public IActionResult DeleteRole()
        {
            return View("AccountManager","ListRole");
        }
    }
}
