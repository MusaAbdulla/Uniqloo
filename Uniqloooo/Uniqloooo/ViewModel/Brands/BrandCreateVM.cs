﻿using System.ComponentModel.DataAnnotations;
using Uniqloooo.Models;
using Uniqloooo.ViewModel.Products;

namespace Uniqloooo.ViewModel.Brands
{
    public class BrandCreateVM
    {
        [MaxLength(64)]
        public string Name { get; set; } = null!;
      
        public static implicit operator Brand(BrandCreateVM vm)
        {
            return new Brand
            {
               
                Name = vm.Name,
               
            
            };
        }
    }
}
