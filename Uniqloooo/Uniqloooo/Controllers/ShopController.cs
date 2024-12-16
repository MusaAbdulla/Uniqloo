using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using Uniqloooo.Context;
using Uniqloooo.Migrations;
using Uniqloooo.Models;
using Uniqloooo.ViewModel.Baskets;
using Uniqloooo.ViewModel.Brands;
using Uniqloooo.ViewModel.Commons;
using Uniqloooo.ViewModel.Products;
using Uniqloooo.ViewModel.Shops;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                amount = amount.Replace("$", "");
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
       public async Task<IActionResult> Details(int? id)
        {
             if(!id.HasValue) return BadRequest();
            var data=await _context.Products
            .Include(x=> x.Images)
            .Include(x=> x.ProductRatings)
            .Include(x=> x.ProductComments)
            .Where(x=>  x.Id == id.Value && !x.IsDeleted )
            .FirstOrDefaultAsync();
            if (data==null) return NotFound();
            string? userId =User.Claims.FirstOrDefault(x=> x.Type ==
            ClaimTypes.NameIdentifier)?.Value;
            if(userId != null)
            {
               var rating= await _context.ProductRatings.Where(x=> x.UserId == userId && x.ProductId==id)
                .Select(x=> x.RatingRate).FirstOrDefaultAsync();
                ViewBag.Rating = rating==0 ? 5 : rating;
            }
            else
            {
                ViewBag.Rating = 5;
            }
            
            return View(data);
        }
        [Authorize]
        public async Task <IActionResult> Rate(int? productId ,int rate=1)
        {
            if (!productId.HasValue) return BadRequest();
            string userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value;
            if (!await _context.Products.AnyAsync(p => p.Id == productId)) return NotFound();
            var rating = await _context.ProductRatings.Where(x => x.ProductId == productId && x.UserId == userId).FirstOrDefaultAsync();
            if (rating is null)
            {
                await _context.ProductRatings.AddAsync(new Models.ProductRating
                {
                    ProductId = productId.Value,
                    RatingRate = rate,
                    UserId = userId
                });
            }
            else
            {
                
                rating.RatingRate = rate;
            }
            
            int RateCount = _context.ProductRatings.Where(x => x.Id == productId).Count();
            double avgRating = 0; 

            if (RateCount != 0)
            {
                avgRating = _context.ProductRatings
                                     .Where(x => x.Id == productId)
                                     .Average(x => x.RatingRate); 
            }

            ViewBag.Rating = rating;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = productId });
        }
 
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Comment(int? productId, string? comment)
        {

            if (!productId.HasValue) return BadRequest();
            string userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value;
            if (!await _context.Products.AnyAsync(p => p.Id == productId)) return NotFound();
            var commenting = await _context.ProductComments.Where(x => x.ProductId == productId && x.UserId == userId).FirstOrDefaultAsync();
            if (commenting is null)
            {
                await _context.ProductComments.AddAsync(new ProductComment
                {
                    CreatedTime = DateTime.Now,
                    IsDeleted = false,
                    UserName = User.Claims.ToString(),
                    ProductId = productId.Value,
                    Comment = comment ,
                    UserId = userId
                });
            }
            else
            {
                commenting.Comment = comment;
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = productId });
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
            return RedirectToAction("Index" ,"Home");
        }
        public IActionResult GetBasket(int id)
        {
            return Json(getBasket());
        }
        List<BasketCookieItemVM> getBasket()
        {
            string? value = (HttpContext.Request.Cookies["basket"]);
            if (value is null) return new();
            return JsonSerializer.Deserialize<List<BasketCookieItemVM>>
               (value) ?? new();
        }
      
        public IActionResult RemoveBasket(int id)
        {
            var basket = getBasket();
           var data= basket.FirstOrDefault(x => x.Id == id);
            if (data != null)
            {
                basket.Remove(data);
            }
            string value = JsonSerializer.Serialize(basket);
            HttpContext.Response.Cookies.Delete("Basket");
            return RedirectToAction("Index" ,"Home");
        }

    }
}
