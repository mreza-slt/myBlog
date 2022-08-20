namespace MyBlog.Plugins.Extentions
{
    public static class StringExtentions
    {
        public static bool IsValidEmail(this string email)
        {
            return !string.IsNullOrEmpty(email) && email.Contains('@');
        }

        public static string ReplaceWithValueIfIsNull(this string text, string value)
        {
            return string.IsNullOrEmpty(text) ? value : text;
        }
    }
}
