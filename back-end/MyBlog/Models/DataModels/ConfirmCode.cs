using MyBlog.Models.Enums;

namespace MyBlog.Models.DataModels
{
    public class ConfirmCode(long userId, int code, CodeType codeType, DateTime createDate, DateTime expireDate)
    {
        /// <summary>شناسه کد ارسال شده</summary>
        public long Id { get; set; }

        public Guid RowId { get; set; }

        /// <summary>شناسه کاربری</summary>
        public long UserId { get; set; } = userId;

        /// <summary>کد ارسال شده</summary>
        public int Code { get; set; } = code;

        /// <summary>مشخص میکند که کد برای ایمیل یا موبایل فرستاده شده است</summary>
        public CodeType CodeType { get; set; } = codeType;

        /// <summary>تاریخ ایجاد کد</summary>
        public DateTime CreateDate { get; set; } = createDate;

        /// <summary>تاریخ انقضاء کد</summary>
        public DateTime ExpireDate { get; set; } = expireDate;

        public User User { get; set; } = null!;
    }
}
