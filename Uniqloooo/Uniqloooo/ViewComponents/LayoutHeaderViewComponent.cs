using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Uniqloooo.Context;
using Uniqloooo.Helpers;
using Uniqloooo.ViewModel.Baskets;

namespace Uniqloooo.ViewComponents
{
    public class LayoutHeaderViewComponent(UniqloDb _context) :ViewComponent
    {
        public async Task<IViewComponentResult> InVokeAsync()
        {
            var basket = BasketHelper.GetBasket(Request);
           var basketItems= await _context.Products
            .Where(x=> basket.Select(y=> y.Id)
            .Contains(x.Id))
            .Select(x=> new BasketItemVm
            {
                Id = x.Id,
                Name = x.Name,
                ImageUrl=x.CoverImage,
                Price = x.SellPrice,
                Discount=x.Discount,
            })
            .ToListAsync();
            foreach(var item in basketItems)           
                item.Count = basket.First(x => x.Id == item.Id).Count;
            return View(basketItems);
        }
    }
}
