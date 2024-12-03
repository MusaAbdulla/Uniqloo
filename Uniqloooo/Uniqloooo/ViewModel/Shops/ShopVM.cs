using Uniqloooo.ViewModel.Brands;
using Uniqloooo.ViewModel.Products;

namespace Uniqloooo.ViewModel.Shops
{
    public class ShopVM
    {
        public IEnumerable<BrandandProductVM> Brands {  get; set; }
        public IEnumerable<ProductListItemVM> Products { get; set; }
        public  int ProductCount { get; set; }
    }
}
