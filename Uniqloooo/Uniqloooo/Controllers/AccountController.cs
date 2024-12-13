using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Identity.Client;
using System.Web.Http;
using Uniqloooo.Extensions;
using Uniqloooo.Models;
using Uniqloooo.ViewModel.Auths;
using Uniqloooo.Views.Enum;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;

namespace Uniqloooo.Areas.Admin.Controllers
{
    
    public class AccountController(UserManager<User> _userManager ,SignInManager<User> 
        _signInManager ,RoleManager<IdentityRole> _roleManager) : Controller
       
    {
        public bool IsAuthenticated => HttpContext.User.Identity?.IsAuthenticated ?? false ;
        public IActionResult Register()
        {
            if (IsAuthenticated) return RedirectToAction("Index","Home");
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> Register(RegisterVM vm)
        {
            if (IsAuthenticated) return RedirectToAction("Index", "Home");
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

                return View();
            }
            var roleResult= await _userManager.AddToRoleAsync(user ,nameof(Roles.User));
            if (!roleResult.Succeeded)
            {
                foreach (var item in roleResult.Errors)
                {
                    ModelState.AddModelError("", item.Description);

                }
                return View();
            }
               return RedirectToAction("Index", "Home");
           
           

        }
        //public async Task <IActionResult> Role()
        //{
        //    foreach(Roles item in Enum.GetValues(typeof(Roles)))
        //    {
        //        await _roleManager.CreateAsync(new IdentityRole(item.GetRole()));
        //    }
        //    return Ok();
        //}
        public IActionResult Login()
        {
            if (IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> Login(LoginVM vm,string? returnUrl=null)
        {
            if (IsAuthenticated) return RedirectToAction("Index", "Home");
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
                if(await _userManager.IsInRoleAsync(user,"Admin"))
                {
                    return RedirectToAction("Index", new {Controller= "DashBoard", Area="Admin"});
                }
                return RedirectToAction("Index", "Home");
               return LocalRedirect(returnUrl);
         
        }
        [Authorize]
        public async Task <IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
        
    }
}
