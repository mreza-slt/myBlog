using System.ComponentModel.DataAnnotations;

namespace MyBlog.CustomAttributes
{
    /// <summary>
    /// چک میکند که شماره موبایل حتما با "09" شروع شود
    /// </summary>
    public class CheckPhoneNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(
            object? value, ValidationContext validationContext)
        {
            var textValue = (string)value!;

            if (!string.IsNullOrEmpty(textValue) && !textValue.StartsWith("09"))
            {
                return new ValidationResult("شماره موبایل باید با 09 شروع شود");
            }

            return ValidationResult.Success;
        }
    }
}
