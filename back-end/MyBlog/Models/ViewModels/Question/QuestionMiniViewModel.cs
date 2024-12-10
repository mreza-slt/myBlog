namespace MyBlog.Models.ViewModels.Question
{
    /// <summary>
    /// نمایش اطلاعات پرسش
    /// </summary>
    public class QuestionMiniViewModel
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
        /// عنوان پرسش
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// متن پرسش
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// تاریخ ثبت پرسش
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
