using System.ComponentModel.DataAnnotations;

namespace Uniqloooo.Models
{
    public class Slider :BaseEntity
    {
        [MaxLength(32)]
        public string Title { get; set; } = null!;
        [MaxLength(128)]
        public string SubTitle { get; set; } = null!;
        public  string? Link { get; set; }
        public string ImageUrl { get; set; } = null!;
    }
}
