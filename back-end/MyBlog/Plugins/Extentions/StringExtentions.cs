using System.Net;
using System.Net.Mail;
using MyBlog.Plugins.Exceptions;

namespace MyBlog.Plugins.Extentions
{
    public static class StringExtentions
    {
        public static bool IsValidEmail(this string email)
        {
            try
            {
                var mailAddress = new MailAddress(email);
                return true; // اگر خطا ندهد، فرمت ایمیل معتبر است.
            }
            catch
            {
                throw new HttpException("ایمیل ثبت شده معتبر نمیباشد لطفا ایمیل خود را به صورت صحیح ویرایش و بعد مجدد تلاش کنید", "", HttpStatusCode.BadRequest);
            }
        }

        public static string ReplaceWithValueIfIsNull(this string text, string value)
        {
            return string.IsNullOrEmpty(text) ? value : text;
        }
    }
}
