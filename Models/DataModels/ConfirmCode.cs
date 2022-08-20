using MyBlog.Models.Enums;

namespace MyBlog.Models.DataModels
{
    public class ConfirmCode
    {
        public ConfirmCode(long userId, int code, CodeType codeType, DateTime createDate, DateTime expireDate)
        {
            this.UserId = userId;
            this.Code = code;
            this.CodeType = codeType;
            this.CreateDate = createDate;
            this.ExpireDate = expireDate;
        }

        /// <summary>شناسه کد ارسال شده</summary>
        public long Id { get; set; }

        public Guid RowId { get; set; }

        /// <summary>شناسه کاربری</summary>
        public long UserId { get; set; }

        /// <summary>کد ارسال شده</summary>
        public int Code { get; set; }

        /// <summary>مشخص میکند که کد برای ایمیل یا موبایل فرستاده شده است</summary>
        public CodeType CodeType { get; set; }

        /// <summary>تاریخ ایجاد کد</summary>
        public DateTime CreateDate { get; set; }

        /// <summary>تاریخ انقضاء کد</summary>
        public DateTime ExpireDate { get; set; }

        public User User { get; set; } = null!;
    }
}
