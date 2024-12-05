using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Uniqloooo.Models;
using Uniqloooo.ViewModel.Auths;

namespace Uniqloooo.Areas.Admin.Controllers
{
    
    public class AccountController(UserManager<User> _userManager ,SignInManager<User> _signInManager) : Controller
    {
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid)
                return View();
            User user = new User()
            { 
                FullName = vm.FullName,
                Email = vm.Email,
                UserName=vm.Username,
                
            };
           var result= await _userManager.CreateAsync(user,vm.Password);
            if (!result.Succeeded)
            {
                foreach(var item in result.Errors)
                {
                    ModelState.AddModelError("",item.Description);
                }
            }
            if (!ModelState.IsValid)
                return View();
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> Login(LoginVM vm,string? returnUrl=null)
        {
            if (!ModelState.IsValid) return View();
            User? user = null;
            if (vm.UsernameorEmail.Contains("@"))
                user =await _userManager.FindByEmailAsync(vm.UsernameorEmail);
            else
                user = await _userManager.FindByNameAsync(vm.UsernameorEmail);
            if (user == null)
            {
                ModelState.AddModelError("", "Username or Password is wrong!");
                return View();
            }
            var result =await _signInManager.PasswordSignInAsync(user,vm.Password ,vm.RememberMe ,true);
            if (!result.Succeeded)
            {
                if(result.IsLockedOut)
                {
                    ModelState.AddModelError("", "wait until"+user.LockoutEnd!
                    .Value.ToString("yyyy-MM-dd HH-mm-ss"));
                }
                if (result.IsNotAllowed)
                {
                    ModelState.AddModelError("", "Username or Password is wrong!");                  
                }
                return View();
            }
            if (string.IsNullOrEmpty(returnUrl))
                return RedirectToAction("Index", "Home");
               return LocalRedirect(returnUrl);
        
        }
    }
}
