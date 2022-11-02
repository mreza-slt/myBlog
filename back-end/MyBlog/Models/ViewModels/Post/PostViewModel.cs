using System.ComponentModel.DataAnnotations;

namespace MyBlog.Models.ViewModels.Post
{
    public class PostViewModel
    {
        /// <summary>
        /// عنوان
        /// </summary>
        [MaxLength(250, ErrorMessage = "تعداد حروف عنوان نباید بیشتر از 250 کاراکتر باشد")]
        [Required(ErrorMessage = "عنوان را وارد کنید")]
        public string Title { get; set; } = null!;

        /// <summary>
        /// متن
        /// </summary>
        [Required(ErrorMessage = "متن پست خود را وارد کنید")]
        public string Text { get; set; } = null!;

        /// <summary>
        /// عکس پست
        /// </summary>
        public string? Image { get; set; }

        /// <summary>
        ///  شناسه دسته بندی موضوعات
        /// </summary>
        [Required(ErrorMessage = "شناسه دسته بندی موضوعات را وارد کنید")]
        public long? SubjectId { get; set; }

        /// <summary>
        /// شناسه موضوع
        /// </summary>
        [Required(ErrorMessage = "شناسه موضوع را وارد کنید")]
        public long? ChildSubjectId { get; set; }
    }
}
