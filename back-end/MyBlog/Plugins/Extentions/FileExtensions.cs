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

        public static string FileToBase64(this IFormFile file)
        {
            using var ms = new MemoryStream();
            file.CopyTo(ms);
            byte[] fileBytes = ms.ToArray();
            string stringFile = Convert.ToBase64String(fileBytes);

            return stringFile;
        }
    }
}
