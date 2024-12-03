using System.ComponentModel.DataAnnotations;
using MyBlog.Plugins.Extentions;

namespace MyBlog.CustomAttributes
{
    /// <summary>
    /// بررسی صحت ایمیل: مقدار می‌تواند خالی باشد.
    /// در صورت پر بودن مقدار، صحت فرمت ایمیل بررسی شده و در صورت نامعتبر بودن، پیام خطا نمایش داده می‌شود.
    /// </summary>
    public class CheckEmailIsEmptyOrFull(string errorMessage) : ValidationAttribute
    {
        private string ErrMessage { get; } = errorMessage;

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
