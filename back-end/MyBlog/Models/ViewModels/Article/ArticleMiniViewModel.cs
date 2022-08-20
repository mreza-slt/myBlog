namespace MyBlog.Models.ViewModels.Article
{
    /// <summary>
    /// نمایش اطلاعات مقاله
    /// </summary>
    public class ArticleMiniViewModel
    {
        /// <summary>
        /// شناسه
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = null!;

        /// <summary>
        /// متن
        /// </summary>
        public string Text { get; set; } = null!;

        /// <summary>
        /// پروفایل
        /// </summary>
        public string? Avatar { get; set; }

        /// <summary>
        /// تاریخ ثبت
        /// </summary>
        public DateTime? RegisterDateTime { get; set; }

        /// <summary>
        /// تعداد بازدید
        /// </summary>
        public int? NumberOfVisits { get; set; }

        /// <summary>
        /// شناسه نویسنده
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// شناسه موضوع والد
        /// </summary>
        public long SubjectId { get; set; }
    }
}
