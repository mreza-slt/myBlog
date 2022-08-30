using System.Dynamic;

namespace MyBlog.Models.ViewModels
{
    /// <summary>
    /// اطلاعات مربوط به درخواست کاربر در صورت موفق بودن یا داشتن خطا
    /// </summary>
    public class ResponseMessageViewModel
    {
        public ResponseMessageViewModel(ExpandoObject? errors, string? message)
        {
            this.Errors = errors;
            this.Message = message;
        }

        /// <summary>پیام دریافتی خطاها</summary>
        public ExpandoObject? Errors { get; set; }

        /// <summary>پیام دریافتی در صورت اجرای موفق درخواست کابر</summary>
        public string? Message { get; set; }
    }
}
