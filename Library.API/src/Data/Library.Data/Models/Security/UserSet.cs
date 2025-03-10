using Microsoft.AspNetCore.Identity;

namespace Library.Data.Models.Security
{
    public class UserSet : IdentityUser<int>
    {

        public ICollection<UserRoleSet> UserRoles { get; set; }

    }
}