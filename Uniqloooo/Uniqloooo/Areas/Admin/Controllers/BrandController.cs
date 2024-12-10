﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Web.Http;
using Uniqloooo.Context;
using Uniqloooo.Models;
using Uniqloooo.ViewModel.Brands;
using Uniqloooo.ViewModel.Products;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;

namespace Uniqloooo.Areas.Admin.Controllers
{
<<<<<<< HEAD
    [Area("Admin"),Authorize]
=======
    [Area("Admin")]
    [Authorize]
>>>>>>> 42ae760e56ef6a7a4df6362ba101bbd7547e117f
    public class BrandController(UniqloDb _context , IWebHostEnvironment _env) : Controller
    {
        public async Task <IActionResult> Index()
        {
            return View(await _context.Brands.ToListAsync());
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
           
            await _context.Brands.AddAsync(brand);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
       
        public IActionResult Update(int? id)
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, BrandCreateVM vm)
        {
            if (!ModelState.IsValid) return View();
            var updt = await _context.Brands.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (updt == null)
                return NotFound();
            updt.Name = vm.Name;
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _context.Brands.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (data == null) return NotFound();
            _context.Brands.Remove(data);
            await _context.SaveChangesAsync(); 
            return RedirectToAction(nameof(Index));
        }
    }
}
