using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.Models.ViewModels.User
{
    /// <summary>
    /// اطلاعات ورود
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// نام کاربری
        /// </summary>
        [DefaultValue("UserName")]
        [Required(ErrorMessage = "نام کاربری را وارد کنید")]
        [MaxLength(250, ErrorMessage = "تعداد حروف نام کاربری نباید بیشتر از 250 کاراکتر باشد")]
        public string UserNameEmailPhone { get; set; } = null!;

        /// <summary>
        /// رمز عبور
        /// </summary>
        [DefaultValue("Password")]
        [DataType(DataType.Password)]
        [MaxLength(100, ErrorMessage = "رمز عبور شما نباید بیشتر از 100 کاراکتر باشد")]
        [MinLength(6, ErrorMessage = "رمز عبور شما نباید کمتر از 6 کاراکتر باشد")]
        [Required(ErrorMessage = "رمز عبور را وارد کنید")]
        public string Password { get; set; } = null!;
    }
}
