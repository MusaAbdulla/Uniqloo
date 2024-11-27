using Microsoft.EntityFrameworkCore;
using Uniqloooo.Models;
namespace Uniqloooo.Context
{
    public class UniqloDb:DbContext
    {
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
      
        public UniqloDb(DbContextOptions opt) : base(opt) { }
     
    }
}
