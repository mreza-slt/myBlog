using System.ComponentModel.DataAnnotations;
using MyBlog.Plugins.Extentions;

namespace MyBlog.CustomAttributes
{
    // Summary:
    // یک پیام حاوی متن ارور دریافت میکند
    // مقدار میتواند خالی باشد. در صورت وارد کردن مقدار تایپ ایمیل چک می شود و اگر تایپ صحیح نباشد متن ارور را نمایش می دهد
    public class CheckEmailIsEmptyOrFull : ValidationAttribute
    {
        public CheckEmailIsEmptyOrFull(string errorMessage)
        {
            this.ErrMessage = errorMessage;
        }

        private string ErrMessage { get; }

        private string GetErrorMessage() =>
            this.ErrMessage;

        protected override ValidationResult? IsValid(
            object? value, ValidationContext validationContext)
        {
            var emailValue = (string)value!;

            if (emailValue != null && emailValue.Length > 0 && !emailValue.IsValidEmail())
            {
                return new ValidationResult(this.GetErrorMessage());
            }

            return ValidationResult.Success;
        }
    }
}
