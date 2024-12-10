namespace MyBlog.Models.ViewModels.Answer
{
    /// <summary>
    /// نمایش اطلاعات پاسخ
    /// </summary>
    public class AnswerMiniViewModel
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
        /// شناسه پرسش
        /// </summary>
        public long QuestionId { get; set; }

        /// <summary>
        /// متن پاسخ
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// تاریخ ثبت پاسخ
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
