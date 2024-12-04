using Uniqloooo.ViewModel.Products;
using Uniqloooo.ViewModel.Sliders;

namespace Uniqloooo.ViewModel.Commons
{
    public class HomeVM
    {
        public IEnumerable<SlideListItemVM> Sliders {  get; set; }
        public IEnumerable<ProductListItemVM>Products { get; set; }
    }
}
