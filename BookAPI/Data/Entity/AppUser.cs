using Microsoft.AspNetCore.Identity;

namespace BookAPI.Data.Entity
{
    public class AppUser:IdentityUser
    {
        public string FullName { get; set; }
    }
}
