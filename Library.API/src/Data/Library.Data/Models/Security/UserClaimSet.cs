using Library.Data.Models.BaseModels;
using Microsoft.AspNetCore.Identity;

namespace Library.Data.Models.Security
{
    public class UserClaimSet : IdentityUserClaim<int>, IBaseEntity
    {
        #region Properties

        public bool IsDeleted { get; set; }
        #endregion]
    }
}
