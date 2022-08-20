﻿using System.ComponentModel.DataAnnotations;

namespace MyBlog.Models.ViewModels.Article
{
    public class ArticleViewModel
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
        [Required(ErrorMessage = "متن مقاله خود را وارد کنید")]
        public string Text { get; set; } = null!;

        /// <summary>
        /// پروفایل
        /// </summary>
        public string? Avatar { get; set; }

        /// <summary>
        ///  شناسه دسته بندی موضوعات
        /// </summary>
        [Required(ErrorMessage = "شناسه دسته بندی موضوعات را وارد کنید")]
        public long? MajorSubjectId { get; set; }

        /// <summary>
        /// شناسه موضوع
        /// </summary>
        [Required(ErrorMessage = "شناسه موضوع را وارد کنید")]
        public long? ForumSubjectId { get; set; }
    }
}
