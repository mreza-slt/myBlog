using Microsoft.AspNetCore.Identity;

namespace MyBlog.Models.DataModels
{
    public partial class UserClaim : IdentityUserClaim<long>
    {
        public Guid RowId { get; set; }
    }
}
