using Microsoft.AspNetCore.Identity;

namespace MyBlog.Models.DataModels
{
    public partial class RoleClaim : IdentityRoleClaim<long>
    {
        public Guid RowId { get; set; }
    }
}
