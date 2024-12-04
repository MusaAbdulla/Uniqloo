using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text.Json;
using Uniqloooo.Context;
using Uniqloooo.ViewModel.Baskets;
using Uniqloooo.ViewModel.Brands;
using Uniqloooo.ViewModel.Commons;
using Uniqloooo.ViewModel.Products;
using Uniqloooo.ViewModel.Shops;

namespace Uniqloooo.Controllers
{
    public class ShopController(UniqloDb _context) : Controller
    {
        public async Task <IActionResult> Index(int? catId, string amount)
        {
            var query = _context.Products.AsQueryable();
            if(catId.HasValue)
            {
                query=query.Where(x=> x.BrandId == catId);
            }
            if(amount!=null)
            {
                var prices = amount
                .Split('-').Select(x=> Convert.ToInt32(x));
                query=query
                .Where(y=> prices.ElementAt(0) <= y.SellPrice && prices.ElementAt(1)>=y.SellPrice);
            }
            ShopVM vm = new ShopVM();
            vm.Brands = await _context.Brands
            .Where(x => !x.IsDeleted)
            .Select(x => new BrandandProductVM
            {
            Id = x.Id,
            Name = x.Name,
            Count = x.Products.Count(),
            } ).ToListAsync();
            vm.Products= await query
            .Take(6)
            .Select(x => new ProductListItemVM
            {
                CoverImage = x.CoverImage,
                Discount = x.Discount,
                SellPrice = x.SellPrice,
                IsInStock = x.Quantity > 0,
                Name = x.Name,
            }
            ).ToListAsync();
            vm.ProductCount= await query.CountAsync();
            return View(vm);
        }
    
        public async Task <IActionResult> AddBasket(int id)
        {
            var basket = getBasket();
            var item = basket.FirstOrDefault(x => x.Id == id);
            if (item != null)     
                item.Count++;
            else
            {
                basket.Add(new BasketCookieItemVM
                {
                    Id = id,
                    Count=1
                });
            }
            string data = JsonSerializer.Serialize(basket);
            HttpContext.Response.Cookies.Append("Basket",data);
            return Ok();
        }
        public IActionResult GetBasket(int id)
        {
            return Json(getBasket());
        }
        List<BasketCookieItemVM> getBasket()
        {
            string? value = (HttpContext.Request.Cookies["basket"]);
            if(value is null) return new();
            return JsonSerializer.Deserialize<List<BasketCookieItemVM>>
               (value) ?? new();
        }
    }
}
