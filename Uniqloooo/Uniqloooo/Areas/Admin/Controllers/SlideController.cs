using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Uniqloooo.Context;
using Uniqloooo.Models;
using Uniqloooo.ViewModel.Sliders;

namespace Uniqloooo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SlideController(UniqloDb _context, IWebHostEnvironment _env) : Controller
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
        public async Task <IActionResult> Create(SliderCreateVM vm)
        {
            if (!ModelState.IsValid)return View(vm);
            if(!vm.File.ContentType.StartsWith("image"))
            {
                ModelState.AddModelError("File", "Format type must be image");
                return View(vm);
            }
            if (vm.File.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("File", "File size must be less than 2 mb");
                return View(vm);
            }
           string newfilename= Path.GetRandomFileName() + Path.GetExtension(vm.File.FileName);
            
            using (Stream stream =System.IO.File.Create(Path.Combine(_env.WebRootPath,"imgs","sliders",newfilename)))
            {
                await vm.File.CopyToAsync(stream);
            }
            Slider slider = new Slider
            {
                ImageUrl = newfilename,
                Title = vm.Title,
                SubTitle = vm.Subtitle!,
                Link = vm.Link
            };
            await _context.sliders.AddAsync(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
           
        }
        public IActionResult Update(int Id)
        {
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> Update(int Id,SliderCreateVM vm)
        {
           var updt= _context.sliders.Where(x => x.Id == Id).FirstOrDefault();
            if (updt != null)
            {
                updt.Title = vm.Title;
                updt.SubTitle = vm.Subtitle;
                updt.CreatedTime = DateTime.Now;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int Id)
        {
            if (Id == null) return BadRequest();
            var data = _context.sliders.Where(x => x.Id == Id).FirstOrDefault();
            string imagePath = Path.Combine(_env.WebRootPath, "imgs", "sliders");
            if (await _context.sliders.AnyAsync(x => x.Id ==Id ))
            {
                _context.sliders.Remove(data);
               await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Hide(int? id)
        {
            var slider = await _context.sliders.FindAsync(id);
            if (slider == null) return NotFound();
            slider.IsDeleted = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }

}
