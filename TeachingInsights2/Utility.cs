using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TeachingInsights2
{
    public class Utility
    {
        public static BitmapSource BuildImage(byte[] imagePixelsArray, int width, int height)
        {
            try
            {
                if (imagePixelsArray != null && imagePixelsArray.Length > 0)
                {
                    var stride = (width * PixelFormats.Bgr24.BitsPerPixel + 7) / 8;
                    var imageSrc = BitmapSource.Create(width, height, 96d, 96d, PixelFormats.Bgr24, null, imagePixelsArray, stride);
                    return imageSrc;
                }

            }
            catch (Exception ex)
            {
                String message = String.IsNullOrEmpty(ex.Message) ? "AffdexMe error encountered." : ex.Message;
                //ShowExceptionAndShutDown(message);
            }
            return null;
        }
    }
}
