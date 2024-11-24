using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Uniqloooo.Context;
using Uniqloooo.Models;
using Uniqloooo.ViewModel.Brands;
using Uniqloooo.ViewModel.Products;

namespace Uniqloooo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController(UniqloDb _context , IWebHostEnvironment _env) : Controller
    {
        public async Task <IActionResult> Index()
        {
            return View(await _context.brands.ToListAsync());
        }
        //RedirectToAction(nameof(Create));
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> Create(BrandCreateVM vm)
        {
            Brand brand = vm;
          
            await _context.brands.AddAsync(brand);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult Update(int Id)
        {
            return View();
        }
        public async Task<IActionResult> Update(int Id, BrandCreateVM vm)
        {
            var updt = _context.brands.Where(x => x.Id == Id).FirstOrDefault();
            if (updt != null)
            {
                updt.Name = vm.Name;
                
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int Id)
        {
            if (Id == null) return BadRequest();
            var data = _context.brands.Where(x => x.Id == Id).FirstOrDefault();
            if (await _context.brands.AnyAsync(x => x.Id == Id))
            {
                _context.brands.Remove(data);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
