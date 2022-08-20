namespace MyBlog.Plugins.Extentions
{
    public static class FileExtensions
    {
        public static byte[] StringToByteArray(this string inputString)
        {
            if (inputString.Contains("base64,"))
            {
                inputString = inputString[(inputString.IndexOf("base64,") + "base64,".Length)..];
            }

            byte[] bytes = Convert.FromBase64String(inputString);

            return bytes;
        }
    }
}
