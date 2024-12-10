using System.ComponentModel.DataAnnotations;

namespace Uniqloooo.ViewModel.Auths
{
    public class LoginVM
    {
       public string UsernameorEmail { get; set; }
        [Required,DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
