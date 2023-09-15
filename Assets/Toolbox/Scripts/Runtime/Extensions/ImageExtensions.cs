using UnityEngine.UI;

namespace Toolbox.Extensions
{
    public static class ImageExtensions
    {
        /// <summary>
        /// Changes alpha value of an image.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="alpha"></param>
        public static void ChangeAlpha(this Image image, float alpha)
        {
            var color = image.color;
            color.a = alpha;
            image.color = color;
        }
    }
}
