using System.ComponentModel.DataAnnotations;
using MyBlog.Models.Enums.Subject;

namespace MyBlog.Models.ViewModels.Subject
{
    /// <summary>
    /// اضافه کردن موضوع فقط توسط ادمین وبسایت
    /// </summary>
    public class SubjectViewModel
    {
        [Required(ErrorMessage = "نام را وارد کنید")]
        public string Name { get; set; } = null!;

        /// <summary>
        ///  نوع دسته بندی
        /// </summary>
        [EnumDataType(typeof(SubjectType), ErrorMessage = "نوع دسته بندی را انتخاب کنید")]
        [Required(ErrorMessage = "مقدار نوع دسته بندی را وارد کنید")]
        public SubjectType SubjectType { get; set; }

        /// <summary>
        /// شناسه والد
        /// </summary>
        public long? ParentId { get; set; }
    }
}
