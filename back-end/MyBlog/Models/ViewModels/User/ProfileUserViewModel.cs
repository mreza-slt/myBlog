using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MyBlog.CustomAttributes;

namespace MyBlog.Models.ViewModels.User
{
    /// <summary>
    /// اطلاعات کاربر برای ویرایش
    /// </summary>
    public class ProfileUserViewModel
    {
        /// <summary>
        /// عنوان - آقای، خانم ...
        /// </summary>
        [DefaultValue(null)]
        [MaxLength(50, ErrorMessage = "تعداد حروف عنوان نباید بیشتر از 50 کاراکتر باشد")]
        public string? Title { get; set; }

        /// <summary>
        /// نام
        /// </summary>
        [DefaultValue("نام")]
        [MaxLength(250, ErrorMessage = "تعداد حروف نام نباید بیشتر از 250 کاراکتر باشد")]
        [Required(ErrorMessage = "نام را وارد کنید")]
        public string Name { get; set; } = null!;

        /// <summary>
        /// نام خانوادگی
        /// </summary>
        [DefaultValue(null)]
        [MaxLength(250, ErrorMessage = "تعداد حروف نام خانوادگی نباید بیشتر از 250 کاراکتر باشد")]
        public string? Surname { get; set; }

        /// <summary>
        /// نام کاربری
        /// </summary>
        [DefaultValue(null)]
        [MaxLength(250, ErrorMessage = "تعداد حروف نام کاربری نباید بیشتر از 250 کاراکتر باشد")]
        public string? UserName { get; set; }

        /// <summary>
        /// ایمیل
        /// </summary>
        [DefaultValue(null)]
        [MaxLength(250, ErrorMessage = "تعداد حروف ایمیل نباید بیشتر از 250 کاراکتر باشد")]
        [CheckEmailIsEmptyOrFull("تایپ ایمیل صحیح نیست")]
        public string? Email { get; set; }

        /// <summary>
        /// شماره موبایل
        /// </summary>
        [DefaultValue("")]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(11, ErrorMessage = "مقدار شماره موبایل باید 11 عدد باشد")]
        [MinLength(11, ErrorMessage = "مقدار شماره موبایل باید 11 عدد باشد")]
        [RegularExpression("^[0-9]\\d*$", ErrorMessage = "شماره موبایل را فقط با اعداد انگلیسی وارد کنید")]
        [CheckPhoneNumber]
        [Required(ErrorMessage = "شماره موبایل را وارد کنید")]
        public string PhoneNumber { get; set; } = null!;

        /// <summary>
        /// عکس پروفایل
        /// </summary>
        [DefaultValue(null)]
        public string? Avatar { get; set; }
    }
}
