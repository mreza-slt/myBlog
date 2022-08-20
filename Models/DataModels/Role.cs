using Microsoft.AspNetCore.Identity;

namespace MyBlog.Models.DataModels
{
    public partial class Role : IdentityRole<long>
    {
        public Guid RowId { get; set; }

        public string Title { get; set; } = null!;
    }
}
