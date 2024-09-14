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
        private readonly UserManager<ApplicationUser> _userManager;
        public AccountManagerController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
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

        [HttpGet]
        public async Task<IActionResult> EditRole(string? id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }


            return View(new EditRoleViewModel
            {

                Name = await _roleManager.GetRoleNameAsync(role)
            });
        }
        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {model.Id} canot be found";
                return View("NotFound");
            }
            else
            {
                role.Name = model.Name;
                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRole", "AccountManager", new { area = "Admin" });
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }
                return View(model);
            }
        }
        public IActionResult DeleteRole()
        {
            return RedirectToAction("AccountManager","ListRole", new {area = "Admin"});
        }
    }
}
