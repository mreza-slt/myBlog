using MyBlog.Models.Enums.Category;

namespace MyBlog.Models.DataModels
{
    /// <summary>
    /// موضوعات
    /// </summary>
    public class Category
    {
        public Category()
        {
            this.Posts = [];
            this.Children = [];
        }

        public Category(int code, string fullCode, string name, CategoryType categoryType, long? parentId)
       : this()
        {
            this.Code = code;
            this.FullCode = fullCode;
            this.Name = name;
            this.CategoryType = categoryType;
            this.ParentId = parentId;
        }

        internal static void Copy(string name, Category category)
        {
            category.Name = name;
        }

        public long Id { get; set; }

        public Guid RowId { get; set; }

        public string Name { get; set; } = null!;

        public int Code { get; set; }

        public string FullCode { get; set; } = null!;

        /// <summary>
        ///  نوع دسته بندی
        /// </summary>
        public CategoryType CategoryType { get; set; }

        /// <summary>
        /// شناسه والد
        /// </summary>
        public long? ParentId { get; set; }

        public Category? Parent { get; set; }

        public ICollection<Category> Children { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}
