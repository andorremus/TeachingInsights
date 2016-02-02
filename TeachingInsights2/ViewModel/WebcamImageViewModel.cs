using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TeachingInsights2.ViewModel
{
    public class WebcamImageViewModel : ViewModelBase
    {
        //public static readonly DependencyProperty RenderedImageProperty =
            //DependencyProperty.Register("RenderedImage", typeof(BitmapSource), 
                                                            //typeof(WebcamImageViewModel),
                                                           // new FrameworkPropertyMetadata(Utility.GetDefaultImage(),RenderedImagePropertyChangedHandler));
        private BitmapSource renderedImage;

        public BitmapSource RenderedImage
        {
            get { return renderedImage; }
            set { renderedImage = value; RaisePropertyChanged("RenderedImage"); }
        }

        //public static void RenderedImagePropertyChangedHandler(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        //{
        //    // Get instance of current control from sender
        //    // and property value from e.NewValue

        //    // Set public property on TaregtCatalogControl, e.g.
        //    ((WebcamImageViewModel)sender).RenderedImage = (BitmapSource)e.NewValue;
        //}

        public WebcamImageViewModel()
        {
            renderedImage = Utility.GetDefaultImage();
        }
        public void StartCapture()
        {

        }
        #region private methods

       
        #endregion


        /* Useful to check if output is as expected
         * public Graphics gr;
         * Console.WriteLine("Updated");
                using (var fileStream = new FileStream("..\\images.jpg", FileMode.Create))
                {
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(value));
                    encoder.Save(fileStream);
                }
         * 
         */
    }
}
