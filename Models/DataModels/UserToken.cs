using Microsoft.AspNetCore.Identity;

namespace MyBlog.Models.DataModels
{
    public partial class UserToken : IdentityUserToken<long>
    {
        public Guid RowId { get; set; }
    }
}
