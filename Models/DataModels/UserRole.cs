using Microsoft.AspNetCore.Identity;

namespace MyBlog.Models.DataModels
{
    public partial class UserRole : IdentityUserRole<long>
    {
        public Guid RowId { get; set; }
    }
}
