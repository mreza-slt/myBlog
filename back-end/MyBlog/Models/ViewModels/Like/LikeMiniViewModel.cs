namespace MyBlog.Models.ViewModels.Like
{
    /// <summary>
    /// نمایش اطلاعات لایک
    /// </summary>
    public class LikeMiniViewModel
    {
        /// <summary>
        /// شناسه
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// شناسه کاربر
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// شناسه کامنت
        /// </summary>
        public long CommentId { get; set; }

        /// <summary>
        /// تاریخ ثبت لایک
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
