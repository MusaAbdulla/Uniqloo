using System.ComponentModel.DataAnnotations;

namespace Uniqloooo.ViewModel.Comments
{
    public class ProductCommentVM
    {
        [MaxLength(255),Required] 
        public string Comment {  get; set; }
    }
}
