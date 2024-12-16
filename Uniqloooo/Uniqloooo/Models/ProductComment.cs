using System.ComponentModel.DataAnnotations;

namespace Uniqloooo.Models
{
    public class ProductComment:BaseEntity
    {
        public string UserName { get; set; }
        [MaxLength(255)]
        public string Comment { get; set; }
        public int? ProductId { get; set; }
        public Product? Product { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
    }
}
