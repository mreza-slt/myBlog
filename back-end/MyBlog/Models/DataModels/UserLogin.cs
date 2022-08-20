using Microsoft.AspNetCore.Identity;

namespace MyBlog.Models.DataModels
{
    public partial class UserLogin : IdentityUserLogin<long>
    {
        public Guid RowId { get; set; }
    }
}
