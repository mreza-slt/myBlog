using System.Drawing;
using MyBlog.Plugins.Extentions;

namespace MyBlog.Services
{
    public class ImageService
    {
        /// <summary>
        /// In this method, it receives the image as a base 64 and a file and a width,
        /// and using the other two methods, it checks the format and changes the size of the image.
        /// </summary>
        /// <returns>imageBase64</returns>
        public string? FixImageSize(string? image, int width)
        {
            if (!string.IsNullOrEmpty(image))
            {
                // check image format
                Image? img = image.CheckImageFormat();

                // get avatarBase64 and FixSize
                return img!.FixedImageSize(width);
            }

            return null;
        }
    }
}
