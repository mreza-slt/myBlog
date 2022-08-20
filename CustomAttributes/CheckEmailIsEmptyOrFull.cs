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

#pragma warning disable SA1202 // Elements should be ordered by access
        protected override ValidationResult? IsValid(
#pragma warning restore SA1202 // Elements should be ordered by access
#pragma warning disable SA1114 // Parameter list should follow declaration
            object? value, ValidationContext validationContext)
#pragma warning restore SA1114 // Parameter list should follow declaration
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
