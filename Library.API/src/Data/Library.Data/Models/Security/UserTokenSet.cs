using Library.Data.Models.BaseModels;
using Microsoft.AspNetCore.Identity;

namespace Library.Data.Models.Security
{
    public class UserTokenSet : IdentityUserToken<int>, IBaseEntity
    {
        public bool IsDeleted { get; set; }
    }
}
