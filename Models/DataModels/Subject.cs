using MyBlog.Models.Enums.Subject;

namespace MyBlog.Models.DataModels
{
    /// <summary>
    /// موضوعات
    /// </summary>
    public class Subject
    {
        public Subject()
        {
            this.Articles = new HashSet<Article>();
            this.Children = new HashSet<Subject>();
        }

        public Subject(int code, string fullCode, string name, SubjectType subjectType, long? parentId)
       : this()
        {
            this.Code = code;
            this.FullCode = fullCode;
            this.Name = name;
            this.SubjectType = subjectType;
            this.ParentId = parentId;
        }

        internal static void Copy(string name, Subject subject)
        {
            subject.Name = name;
        }

        public long Id { get; set; }

        public Guid RowId { get; set; }

        public string Name { get; set; } = null!;

        public int Code { get; set; }

        public string FullCode { get; set; } = null!;

        /// <summary>
        ///  نوع دسته بندی
        /// </summary>
        public SubjectType SubjectType { get; set; }

        /// <summary>
        /// شناسه والد
        /// </summary>
        public long? ParentId { get; set; }

        public Subject? Parent { get; set; }

        public ICollection<Subject> Children { get; set; }

        public ICollection<Article> Articles { get; set; }
    }
}
