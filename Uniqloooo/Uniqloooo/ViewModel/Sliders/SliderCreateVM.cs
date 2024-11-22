using System.ComponentModel.DataAnnotations;
using Uniqloooo.Models;

namespace Uniqloooo.ViewModel.Sliders
{
    public class SliderCreateVM 
    {
        [MaxLength(32,ErrorMessage="Başlıq 32 simvoldan çox ola bilməz!"),Required]
        public string Title { get; set; }
        [Required]
        public string Subtitle { get; set; }
        public string? Link { get; set; }
        [Required(ErrorMessage ="Fayl Seçilməyib")]
        public IFormFile File { get; set; }
    }
}
