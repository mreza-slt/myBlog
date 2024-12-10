using System.ComponentModel.DataAnnotations;

namespace MyBlog.Models.ViewModels.Comment
{
    public class CommentViewModel
    {
        [Required(ErrorMessage = "شناسه پست را وارد کنید")]
        public long? PostId { get; set; }

        [Required(ErrorMessage = "لطفا کامنت خود را وارد کنید")]
        public string Content { get; set; } = null!;

        public long? ParentId { get; set; }
    }
}
