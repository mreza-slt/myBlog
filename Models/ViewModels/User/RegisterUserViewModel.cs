using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MyBlog.CustomAttributes;

namespace MyBlog.Models.ViewModels.User
{
    /// <summary>
    /// اطلاعات کاربر
    /// </summary>
    public class RegisterUserViewModel
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
        /// رمز عبور
        /// </summary>
        [DefaultValue("Password")]
        [DataType(DataType.Password)]
        [MaxLength(100, ErrorMessage = "رمز عبور شما نباید بیشتر از 100 کاراکتر باشد")]
        [MinLength(6, ErrorMessage = "رمز عبور شما نباید کمتر از 6 کاراکتر باشد")]
        [Required(ErrorMessage = "رمز عبور را وارد کنید")]
        public string Password { get; set; } = null!;

        /// <summary>
        /// تکرار رمز عبور
        /// </summary>
        [DefaultValue("Password")]
        [Compare("Password", ErrorMessage = "تکرار رمز عبور باید با رمز اصلی یکی باشد")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "تکرار رمز عبور را وارد کنید")]
        public string PasswordConfirm { get; set; } = null!;
    }
}
