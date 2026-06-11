using FinalExam.Models;
using FinalExam.Utilities.Enums;
using FinalExam.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FinalExam.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            AppUser user = new AppUser
            {
                Name = vm.Name,
                Surname = vm.Surname,
                Email = vm.Email,
                UserName = vm.UserName
            };

            IdentityResult result = await _userManager.CreateAsync(user, vm.Password);
            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View();
            }
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");

        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            AppUser? user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == vm.UserNameOrEmail || u.Email == vm.UserNameOrEmail);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User is not found");
                return NotFound();
            }
            var result = await _signInManager.PasswordSignInAsync(user, vm.Password, isPersistent: false, true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Your account is blocked");
                return View();
            }
            if (result.Succeeded)
            {
                
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            ModelState.AddModelError(string.Empty, "Password is incorrect");
            return View();

        }

        public async Task<IActionResult> CreateRole()
        {

            foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
            {
                if (!await _roleManager.RoleExistsAsync(role.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });
                }
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
