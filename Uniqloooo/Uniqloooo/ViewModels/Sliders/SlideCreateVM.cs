using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Uniqloooo.ViewModels.Sliders
{
    public class SlideCreateVM 
    {
        [MaxLength(32,ErrorMessage ="Başlıq 32 simvoldan çox ola bilməz")]
            [Required]
        public string Title { get; set; }
        [Required]
        public string Subtitle { get; set; }
        public string? Link { get; set; }
        [Required(ErrorMessage ="Fayl Seçilməyib")]
        public IFormFile file { get; set; }
    }
}
