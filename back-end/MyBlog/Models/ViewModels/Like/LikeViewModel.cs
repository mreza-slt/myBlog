using System.ComponentModel.DataAnnotations;

namespace MyBlog.Models.ViewModels.Like
{
    public class LikeViewModel
    {
        /// <summary>
        /// شناسه کاربر
        /// </summary>
        [Required(ErrorMessage = "شناسه کاربر را وارد کنید")]
        public long UserId { get; set; }

        /// <summary>
        /// شناسه کامنت
        /// </summary>
        [Required(ErrorMessage = "شناسه کامنتت را وارد کنید")]
        public long CommentId { get; set; }

    }
}
