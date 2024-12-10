using System.ComponentModel.DataAnnotations;

namespace Uniqloooo.ViewModel.Auths
{
    public class RegisterVM
    {
        [Required, MaxLength(64)]
        public string FullName { get; set; } = null!;
        [Required, MaxLength(128),DataType(DataType.EmailAddress), EmailAddress]
        public string Email { get; set; } = null!;
        [Required, MaxLength(32),DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, MaxLength(128)]
        public string Username { get; set; } = null!;
        [Required, MaxLength(32),DataType(DataType.Password),Compare(nameof(Password))]
        public string RePassword { get; set; }   
    }
}
