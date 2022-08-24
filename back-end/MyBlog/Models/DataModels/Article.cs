using System.ComponentModel.DataAnnotations;

namespace MyBlog.Models.DataModels
{
    /// <summary>
    /// پست
    /// </summary>
    public class Post
    {
        public Post(string title, string text, string? avatar, long userId, long subjectId)
        {
            this.Title = title;
            this.Text = text;
            this.Avatar = avatar;
            this.UserId = userId;
            this.SubjectId = subjectId;

            this.RegisterDateTime = DateTime.Now;
        }

        public long Id { get; set; }

        public Guid RowId { get; set; }

        /// <summary>
        /// عنوان
        /// </summary>
        [MaxLength(250)]
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
        /// شناسه موضوع اصلی
        /// </summary>
        public long SubjectId { get; set; }

        public User User { get; set; } = null!;

        public Subject Subject { get; set; } = null!;
    }
}
