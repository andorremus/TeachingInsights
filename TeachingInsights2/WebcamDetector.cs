using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace TeachingInsights2
{
    class WebcamDetector
    {
        private BitmapSource cameraFrameFeed;

        public BitmapSource CameraFrameFeed
        {
            get { return cameraFrameFeed;}
            set { cameraFrameFeed = value; }
        }
        public WebcamDetector()
        {

        }

    }
}
