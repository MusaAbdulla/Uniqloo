using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Uniqloooo.Context;
using Uniqloooo.Extensions;
using Uniqloooo.Models;
using Uniqloooo.ViewModel.Products;

namespace Uniqloooo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController(UniqloDb _context, IWebHostEnvironment _env) : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction(nameof(Create));
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.brands.Where(x => !x.IsDeleted).ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVM vm)
        {
            if (vm.File != null)
            {
                if (!vm.File.IsValidType("image"))
                    ModelState.AddModelError("File", "File must be an image");
                if (!vm.File.IsValidSize(400))
                    ModelState.AddModelError("File", "File must be less than 400kb");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _context.brands.Where(x => !x.IsDeleted).ToListAsync();
                return View(vm);
            }
            if (!await _context.brands.AnyAsync(x => x.Id == vm.BrandId))
            {
                ViewBag.Categories = await _context.brands.Where(x => !x.IsDeleted).ToListAsync();
                ModelState.AddModelError("BrandId", "Brand not found");
                return View();
            }
            Product product = vm;
            product.CoverImage = await vm.File!.UploadAsync(_env.WebRootPath, "imgs", "products");
            await _context.products.AddAsync(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
