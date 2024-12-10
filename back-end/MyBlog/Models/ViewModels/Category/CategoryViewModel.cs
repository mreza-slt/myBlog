using System.ComponentModel.DataAnnotations;
using MyBlog.Models.Enums.Category;

namespace MyBlog.Models.ViewModels.Category
{
    /// <summary>
    /// اضافه کردن موضوع فقط توسط ادمین وبسایت
    /// </summary>
    public class CategoryViewModel
    {
        [Required(ErrorMessage = "نام را وارد کنید")]
        public string Name { get; set; } = null!;

        /// <summary>
        ///  نوع دسته بندی
        /// </summary>
        [EnumDataType(typeof(CategoryType), ErrorMessage = "نوع دسته بندی را انتخاب کنید")]
        [Required(ErrorMessage = "مقدار نوع دسته بندی را وارد کنید")]
        public CategoryType CategoryType { get; set; }

        /// <summary>
        /// شناسه والد
        /// </summary>
        public long? ParentId { get; set; }
    }
}
