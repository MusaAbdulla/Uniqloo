using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Uniqloooo.Models;
namespace Uniqloooo.Context
{
<<<<<<< HEAD
    public class UniqloDb: IdentityDbContext<User>
=======
    public class UniqloDb:IdentityDbContext<User>
>>>>>>> 42ae760e56ef6a7a4df6362ba101bbd7547e117f
    {
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<ProductImage> ProductImage{ get;  set; }
        public UniqloDb(DbContextOptions opt) : base(opt) { }
     
    }
}
