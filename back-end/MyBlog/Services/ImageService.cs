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
        public string? FixImageSize(IFormFile? imageFile, int width)
        {
            if (imageFile != null)
            {
                // check image format
                Image? image = imageFile.CheckImageFormat();

                // get avatarBase64 and FixSize
                return image!.FixedImageSize(width);
            }

            return null;
        }
    }
}
