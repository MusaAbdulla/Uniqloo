using Microsoft.AspNetCore.Identity;

namespace Uniqloooo.Models
{
    public class User :IdentityUser
    {
        public string FullName { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? Address {  get; set; }
    }
}
