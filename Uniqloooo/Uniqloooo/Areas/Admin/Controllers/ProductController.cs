using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using NuGet.Packaging;
using System.Linq;
using System.Web.Http;
using System.Xml;
using Uniqloooo.Context;
using Uniqloooo.Extensions;
using Uniqloooo.Models;
using Uniqloooo.ViewModel.Products;
using Uniqloooo.ViewModel.Sliders;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;

namespace Uniqloooo.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class ProductController(IWebHostEnvironment _env, UniqloDb _context) : Controller
{
    public async Task<IActionResult> Index()
    {
        return View(await _context.Products.Include(x => x.Brand).ToListAsync());
    }
    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = await _context.Brands.Where(x => !x.IsDeleted).ToListAsync();
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
        if (vm.OtherFiles != null && vm.OtherFiles.Any())
        {
            if (!vm.OtherFiles.All(x => x.IsValidType("image")))
            {
                string fileNames = string.Join(',', vm.OtherFiles.Where(x => !x.IsValidType("image")).Select(x => x.FileName));
                ModelState.AddModelError("OtherFiles", fileNames + " is (are) not an image");
            }
            if (!vm.OtherFiles.All(x => x.IsValidSize(400)))
            {
                string fileNames = string.Join(',', vm.OtherFiles.Where(x => !x.IsValidSize(400)).Select(x => x.FileName));
                ModelState.AddModelError("OtherFiles", fileNames + " is (are) bigger than 400kb");
            }
        }
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = await _context.Brands.Where(x => !x.IsDeleted).ToListAsync();
            return View(vm);
        }
        if (!await _context.Brands.AnyAsync(x => x.Id == vm.BrandId))
        {
            ViewBag.Categories = await _context.Brands.Where(x => !x.IsDeleted).ToListAsync();
            ModelState.AddModelError("BrandId", "Brand not found");
            return View();
        }
        Product product = vm;
        product.CoverImage = await vm.File!.UploadAsync(_env.WebRootPath, "imgs", "products");
        product.Images = vm.OtherFiles?.Select(x => new ProductImage
        {
            
            ImageUrl = x.UploadAsync(_env.WebRootPath, "imgs", "products").Result
        }).ToList();
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(int? id)
    {
        if (id is null) return BadRequest();
        var data = await _context.Products
            .Where(x => x.Id == id)
            .Select(x => new ProductUpdateVm
            {
                Id = x.Id,
                BrandId = x.BrandId ?? 0,
                CostPrice = x.CostPrice,
                Description = x.Description,
                Discount = x.Discount,
                FileUrl = x.CoverImage,
                Name = x.Name,
                Quantity = x.Quantity,
                SellPrice = x.SellPrice,
                OtherFileUrls = x.Images!.Select(y => y.ImageUrl)
            })
            .FirstOrDefaultAsync();

        if (data is null) return NotFound();
        ViewBag.Categories= await _context.Brands.Where(x => !x.IsDeleted)
        .ToListAsync();
        return View(data);
    }
    [HttpPost]
    public async Task<IActionResult> Update(int? id, ProductUpdateVm vm )
    {
       
        var updt = await _context.Products.Where(x => x.Id == id)
        .Include(x=> x.Images)
        .FirstOrDefaultAsync();
        if (updt == null) return NotFound();
        updt.Images.AddRange(vm.OtherFiles.Select(x => new ProductImage
        {

            ImageUrl = x.UploadAsync(_env.WebRootPath, "imgs", "products").Result
        }).ToList());
        updt.Name = vm.Name;
        updt.Description = vm.Description;
        updt.SellPrice = vm.SellPrice;
        updt.CostPrice = vm.CostPrice;
        updt.Quantity = vm.Quantity;
        updt.Discount = vm.Discount;
        updt.BrandId = vm.BrandId;
        updt.CreatedTime = DateTime.Now;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));           
    }
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return BadRequest();
        var data = await _context.Products.Where(x => x.Id == id).FirstOrDefaultAsync();
        string imagePath = Path.Combine(_env.WebRootPath, "imgs", "produtcs");
        if (data == null) return NotFound();
        _context.Products.Remove(data);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    [HttpPost]
    public async Task<IActionResult> DeleteImgs(int id, IEnumerable<string> imgNames)
    {
        int result = await _context.ProductImage.Where(x => imgNames.Contains(x.ImageUrl)).ExecuteDeleteAsync();
        if (result > 0)
        {
            //serverden (komputerden (fayllardan)) kohne shekilleri sil
        }
        return RedirectToAction(nameof(Update), new { id });
    }
}

