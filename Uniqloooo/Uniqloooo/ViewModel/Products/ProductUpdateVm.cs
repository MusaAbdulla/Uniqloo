using System.ComponentModel.DataAnnotations;
using Uniqloooo.Models;

namespace Uniqloooo.ViewModel.Products
{
    public class ProductUpdateVm
    {
        public int Id { get; set; }
        [MaxLength(64)]
        public string Name { get; set; }
        [MaxLength(512)]
        public string Description { get; set; }
        public string CoverImage { get; set; }
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
        [DataType("decimal 18,2")]
        public decimal CostPrice { get; set; }
        [DataType("decimal 18,2")]
        public decimal SellPrice { get; set; }
        [Range(0, 100)]
        public int Discount { get; set; }
        public IFormFile File { get; set; }
        public string? FileUrl { get; set; }
        public IEnumerable<string>?  OtherFileUrls { get; set; }
        public int BrandId { get; set; }
        public Brand? Brand { get; set; }
        public ICollection<IFormFile> OtherFiles { get; set; }
    }
   
}
