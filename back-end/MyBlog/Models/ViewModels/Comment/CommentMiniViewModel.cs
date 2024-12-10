namespace MyBlog.Models.ViewModels.Comment
{
    /// <summary>
    /// نمایش اطلاعات کامنت
    /// </summary>
    public class CommentMiniViewModel
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
        /// شناسه پست
        /// </summary>
        public long PostId { get; set; }

        /// <summary>
        /// محتوای کامنت
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// تاریخ ثبت کامنت
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// شناسه والد
        /// </summary>
        public long? ParentId { get; set; }
    }
}
