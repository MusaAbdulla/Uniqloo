using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Uniqloooo.Context;
using Uniqloooo.Models;
using Uniqloooo.ViewModels.Sliders;

namespace Uniqloooo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController(UniqloDb _context, IWebHostEnvironment _env) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await _context.sliders.ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(SlideCreateVM vm)
        {
            if (!ModelState.IsValid) return View(vm);
            if (!vm.file.ContentType.StartsWith("image"))
            {
                ModelState.AddModelError("File", "Format type must be image");
                return View(vm);
            }
            if (vm.file.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("File", "File size must be less than 2 mb");
                return View(vm);
            }
            string newFileName = Path.GetRandomFileName() + Path.GetExtension(vm.file.FileName);

            using (Stream stream = System.IO.File.Create(Path.Combine(_env.WebRootPath, "imgs", "sliders", newFileName)))
            {
                await vm.file.CopyToAsync(stream);
            }
            Slider slider = new Slider
            {
                ImageUrl = newFileName,
                Title = vm.Title,
                SubTitle = vm.Subtitle!,
                Link = vm.Link
            };
            await _context.sliders.AddAsync(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
