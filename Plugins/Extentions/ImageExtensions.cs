using System.Drawing;
using System.Drawing.Imaging;

namespace MyBlog.Plugins.Extentions
{
    public static class ImageExtensions
    {
        public static string ImageToBase64(this Image image, ImageFormat rowFormat)
        {
            using MemoryStream m = new();
            image.Save(m, rowFormat);
            byte[] imageBytes = m.ToArray();
            string base64String = Convert.ToBase64String(imageBytes);
            return $"data:image/{rowFormat};base64," + base64String;
        }

        public static Image ByteArrayToImage(this byte[] byteArrayIn)
        {
            Image image;

            using MemoryStream ms = new(byteArrayIn);

            image = Image.FromStream(ms);

            return image;
        }

        public static string FixedImageSize(this Image image, int width)
        {
            if (image.Width < width)
            {
                Image oldImage = image.GetThumbnailImage(image.Width, image.Height, null, IntPtr.Zero);

                string originalImage = oldImage.ImageToBase64(image.RawFormat);

                image.Dispose();

                return originalImage;
            }

            int w = image.Width;
            int h = image.Height;
            int height = (width * h) / w;

            Image newImage = image.GetThumbnailImage(width, height, null, IntPtr.Zero);

            string imageString = newImage.ImageToBase64(image.RawFormat);

            image.Dispose();

            return imageString;
        }

        public static Image? CheckImageFormat(this string imageString)
        {
            byte[] byteImage = imageString.StringToByteArray();

            if (byteImage.Length > 0)
            {
                Image image = byteImage.ByteArrayToImage();

                return image;
            }
            else
            {
                throw new ArgumentException(null, nameof(imageString));
            }
        }
    }
}
