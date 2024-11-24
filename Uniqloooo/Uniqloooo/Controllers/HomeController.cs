using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Uniqloooo.Context;
using Uniqloooo.Models;
using Uniqloooo.ViewModel.Commons;
using Uniqloooo.ViewModel.Products;
using Uniqloooo.ViewModel.Sliders;

namespace Uniqloooo.Controllers
{
    public class HomeController(UniqloDb _context) : Controller
    {

        public async Task<IActionResult> Index()
        {
            HomeVM vm = new ();
            vm.Sliders = await _context.sliders.Select(x => new SlideListItemVM
            {
                ImageUrl=x.ImageUrl,
                Title=x.Title,
                SubTitle=x.SubTitle,
                Link=x.Link,

            }).ToListAsync();
            vm.Products= await _context.products.Select(x=> new ProductListItemVM
            {
                CoverImage=x.CoverImage,
                Discount=x.Discount,
                SellPrice=x.SellPrice,
                IsInStock=x.Quantity>0,
                Name=x.Name,
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
    }
}
