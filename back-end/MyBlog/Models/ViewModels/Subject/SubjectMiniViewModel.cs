using System.ComponentModel.DataAnnotations;
using MyBlog.Models.Enums.Subject;

namespace MyBlog.Models.ViewModels.Subject
{
    /// <summary>
    /// نمایش موضوعات و دسته بندی ها
    /// </summary>
    public class SubjectMiniViewModel
    {
        /// <summary>
        /// شناسه
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// نام
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// کد
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// کد کامل
        /// </summary>
        public string FullCode { get; set; } = null!;

        /// <summary>
        ///  نوع دسته بندی
        /// </summary>
        public SubjectType SubjectType { get; set; }

        /// <summary>
        /// شناسه والد
        /// </summary>
        public long? ParentId { get; set; }
    }
}
