namespace MyBlog.Models.ViewModels.Post
{
    /// <summary>
    /// نمایش اطلاعات پست
    /// </summary>
    public class PostMiniViewModel
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
        /// عکس
        /// </summary>
        public string? Image { get; set; }

        /// <summary>
        /// عکس کاریر
        /// </summary>
        public string? UserAvatar { get; set; }

        /// <summary>
        /// تاریخ ثبت
        /// </summary>
        public DateTime? RegisterDateTime { get; set; }

        /// <summary>
        /// تعداد بازدید
        /// </summary>
        public int? NumberOfVisits { get; set; }

        /// <summary>
        /// نام نویسنده
        /// </summary>
        public string UserName { get; set; } = null!;

        /// <summary>
        ///  نام دسته بندی موضوع
        /// </summary>
        public string SubjectName { get; set; } = null!;
    }
}
