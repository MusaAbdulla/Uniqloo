using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Uniqloooo.Context;
using Uniqloooo.ViewModel.Commons;
using Uniqloooo.ViewModel.Products;
using Uniqloooo.ViewModel.Sliders;

namespace Uniqloooo.Controllers
{
    public class HomeController(UniqloDb _context) : Controller
    {

        public async Task<IActionResult> Index()
        {
            HomeVM vm = new();
            vm.Sliders = await _context.Sliders
                .Where(x => !x.IsDeleted)
                .Select(x => new SlideListItemVM
                {
                    ImageUrl = x.ImageUrl,
                    Title = x.Title,
                    SubTitle = x.SubTitle,
                    Link = x.Link,

                }).ToListAsync();
            vm.Products = await _context.Products.Select(x => new ProductListItemVM
            {
                Id = x.Id,
                CoverImage = x.CoverImage,
                Discount = x.Discount,
                SellPrice = x.SellPrice,
                IsInStock = x.Quantity > 0,
                Name = x.Name,
            }
            ).ToListAsync();

            return View(vm);
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Shop()
        {
            return View();
        }
        public void SetSession(string key, string value)
        {
            HttpContext.Session.SetString(key, value);
        }
        public IActionResult GetSession(string key)
        {
            return Content(HttpContext.Session.GetString(key) ?? string.Empty);
        }
        public void SetCookie(string key, string value)
        {
            HttpContext.Response.Cookies.Append(key, value, new CookieOptions
            {
                MaxAge = TimeSpan.FromMinutes(2)

            });
        }
        public IActionResult GetCookie(string key)
        {
            return Content(HttpContext.Request.Cookies[key]);
        }
    }
}
