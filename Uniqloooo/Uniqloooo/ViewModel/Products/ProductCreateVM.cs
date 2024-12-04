using Microsoft.AspNetCore.Http.Metadata;
using System.ComponentModel.DataAnnotations;
using Uniqloooo.Models;

namespace Uniqloooo.ViewModel.Products
{
    public class ProductCreateVM
    {
        [MaxLength(64)]
        public string Name { get; set; }                     
        [MaxLength(512)]
        public string Description { get; set; }
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
        [DataType("decimal 18,2")]
        public decimal CostPrice { get; set; }
        [DataType("decimal 18,2")]
        public decimal SellPrice { get; set; }
        [Range(0, 100)]
        public int Discount { get; set; }
        public IFormFile File { get; set; }
        public int BrandId { get; set; }
        public ICollection<IFormFile>? OtherFiles { get; set; }

        public static implicit operator Product(ProductCreateVM vm)
        {
            return new Product
            {
             
                BrandId = vm.BrandId,
                CostPrice = vm.CostPrice,
                Description = vm.Description,
                Discount = vm.Discount,
                Name = vm.Name,
                Quantity = vm.Quantity,
                SellPrice = vm.SellPrice,
              
                
            };
        }


    }
}
