using System.ComponentModel.DataAnnotations;

namespace MyBlog.Models.DataModels
{
    /// <summary>
    /// پست
    /// </summary>
    public class Post(string title, string text, string? image, long userId, long categoryId)
    {
        public long Id { get; set; }

        public Guid RowId { get; set; }

        /// <summary>
        /// عنوان
        /// </summary>
        [MaxLength(250)]
        public string Title { get; set; } = title;

        /// <summary>
        /// متن
        /// </summary>
        public string Text { get; set; } = text;

        /// <summary>
        /// پروفایل
        /// </summary>
        public string? Image { get; set; } = image;

        /// <summary>
        /// تاریخ ثبت
        /// </summary>
        public DateTime? RegisterDateTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// تعداد بازدید
        /// </summary>
        public int? NumberOfVisits { get; set; }

        /// <summary>
        /// شناسه نویسنده
        /// </summary>
        public long UserId { get; set; } = userId;

        /// <summary>
        /// شناسه موضوع اصلی
        /// </summary>
        public long CategoryId { get; set; } = categoryId;

        public User User { get; set; } = null!;

        public Category Category { get; set; } = null!;

        public ICollection<Comment> Comments { get; set; } = [];
    }
}
