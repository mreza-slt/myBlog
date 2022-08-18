using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MyBlog.CustomAttributes;

namespace MyBlog.Models.ViewModels.User
{
    public class ProfileUserViewModel
    {
        [DefaultValue(null)]
        [MaxLength(50, ErrorMessage = "تعداد حروف عنوان نباید بیشتر از 50 کاراکتر باشد")]
        public string? Title { get; set; }

        [DefaultValue("نام")]
        [MaxLength(250, ErrorMessage = "تعداد حروف نام نباید بیشتر از 250 کاراکتر باشد")]
        [Required(ErrorMessage = "نام را وارد کنید")]
        public string Name { get; set; } = null!;

        [DefaultValue(null)]
        [MaxLength(250, ErrorMessage = "تعداد حروف نام خانوادگی نباید بیشتر از 250 کاراکتر باشد")]
        public string? Surname { get; set; }

        [DefaultValue(null)]
        [MaxLength(250, ErrorMessage = "تعداد حروف نام کاربری نباید بیشتر از 250 کاراکتر باشد")]
        public string? UserName { get; set; }

        [DefaultValue(null)]
        [MaxLength(250, ErrorMessage = "تعداد حروف ایمیل نباید بیشتر از 250 کاراکتر باشد")]
        [CheckEmailIsEmptyOrFull("تایپ ایمیل صحیح نیست")]
        public string? Email { get; set; }

        [DefaultValue("")]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(11, ErrorMessage = "مقدار شماره موبایل باید 11 عدد باشد")]
        [MinLength(11, ErrorMessage = "مقدار شماره موبایل باید 11 عدد باشد")]
        [RegularExpression("^[0-9]\\d*$", ErrorMessage = "شماره موبایل را فقط با اعداد انگلیسی وارد کنید")]
        [CheckPhoneNumber]
        [Required(ErrorMessage = "شماره موبایل را وارد کنید")]
        public string PhoneNumber { get; set; } = null!;

        [DefaultValue("")]
        public string? Avatar { get; set; }
    }
}
