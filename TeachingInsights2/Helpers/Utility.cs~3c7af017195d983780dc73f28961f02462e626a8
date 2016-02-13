using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TeachingInsights2
{

    //public enum UserType
    //{
    //    Teacher,
    //    Student,
    //    Admin
    //}
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

        public static BitmapSource GetDefaultImage()
        {
            int width = 128;
            int height = width;
            int stride = width / 8;
            byte[] pixels = new byte[height * stride];

            // Try creating a new image with a custom palette.
            List<System.Windows.Media.Color> colors = new List<System.Windows.Media.Color>();
            colors.Add(System.Windows.Media.Colors.Red);
            colors.Add(System.Windows.Media.Colors.Blue);
            colors.Add(System.Windows.Media.Colors.Green);
            BitmapPalette myPalette = new BitmapPalette(colors);

            // Creates a new empty image with the pre-defined palette
            BitmapSource image = BitmapSource.Create(
                                                     width, height,
                                                     96, 96,
                                                     PixelFormats.Indexed1,
                                                     myPalette,
                                                     pixels,
                                                     stride);
            return image;
        }

        public static BitmapSource Convert(System.Drawing.Bitmap bitmap)
        {
            var bitmapData = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);

            var bitmapSource = BitmapSource.Create(
                bitmapData.Width, bitmapData.Height, 96, 96, PixelFormats.Bgr24, null,
                bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

            bitmap.UnlockBits(bitmapData);
            return bitmapSource;
        }

        public static Bitmap BitmapFromSource(BitmapSource bitmapsource)
        {
            Bitmap bitmap;
            using (var outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                enc.Save(outStream);
                bitmap = new Bitmap(outStream);
            }
            return bitmap;
        }



        public static String GetClassifierDataFolder()
        {
            String affdexClassifierDir = "C:\\Projects\\TeachingInsights2\\TeachingInsights2\\data\\";
            if (String.IsNullOrEmpty(affdexClassifierDir))
            {
                //ShowExceptionAndShutDown("AFFDEX_DATA_DIR environment variable (Classifier Data Directory) is not set");
            }

            DirectoryInfo directoryInfo = new DirectoryInfo(affdexClassifierDir);
            if (!directoryInfo.Exists)
            {
                //ShowExceptionAndShutDown("AFFDEX_DATA_DIR (Classifier Data Directory) is set to an invalid folder location");
            }

            return affdexClassifierDir;
        }

        public static String GetAffdexLicense()
        {
            String licensePath = String.Empty;
            licensePath = "C:\\Projects\\TeachingInsights2\\TeachingInsights2\\license\\sdk_andor.remus@gmail.com.license";
            if (String.IsNullOrEmpty(licensePath))
            {
                //ShowExceptionAndShutDown("AFFDEX_LICENSE_DIR environment variable (Affdex License Folder) is not set");
            }

            return licensePath;
        }

    }
}
