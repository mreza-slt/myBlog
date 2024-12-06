using MyBlog.Models.Enums.Category;

namespace MyBlog.Models.ViewModels.Category
{
    /// <summary>
    /// نمایش موضوعات و دسته بندی ها
    /// </summary>
    public class CategoryMiniViewModel
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
        public CategoryType CategoryType { get; set; }

        /// <summary>
        /// شناسه والد
        /// </summary>
        public long? ParentId { get; set; }
    }
}
