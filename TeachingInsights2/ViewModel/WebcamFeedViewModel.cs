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
using Affdex;
using System.Windows.Input;
using System.Diagnostics;
using System.Collections.ObjectModel;

using AForge.Video.DirectShow;
using AForge.Video;
using AForge.Video.FFMPEG;

namespace TeachingInsights2.ViewModel
{
    public class WebcamFeedViewModel : ViewModelBase,Affdex.ImageListener

    {

        #region private methods
        
        private BitmapSource renderedImage;
        private CameraDetector cameraDetector;
        private ICommand startRecCommand;
        private ICommand stopRecCommand;
        private ViewModelLocator vmLocator;
        private VideoFileWriter videoWriter;

        private int counter = 0;



        #endregion

        #region public members
        public BitmapSource RenderedImage
        {
            get { return renderedImage; }
            set { renderedImage = value; RaisePropertyChanged("RenderedImage"); }
        }

        //public ObservableCollection<Point> Collection { get; set; }
        //public ObservableCollection<Point> BrowFurrowCollection { get; set; }

        #endregion

        public WebcamFeedViewModel()
        {
            vmLocator = new ViewModelLocator();   
            //Collection = new ObservableCollection<Point>();
            //BrowFurrowCollection = new ObservableCollection<Point>();
        }

        public ICommand StartRecCommand
        {
            get
            {
                if (startRecCommand == null)
                    startRecCommand = new RelayCommand(param => this.StartCapture(), null);
                return startRecCommand;
            }
        }

        public ICommand StopRecCommand
        {
            get
            {
                if (stopRecCommand == null)
                    stopRecCommand = new RelayCommand(param => this.StopCameraProcessing(), null);
                return stopRecCommand;
            }
        }
        private void StopCameraProcessing()
        {
            try
            {
                if ((cameraDetector != null) && (cameraDetector.isRunning()))
                {
                    cameraDetector.stop();
                    cameraDetector.Dispose();
                    cameraDetector = null;
                }
                if (videoWriter != null)
                {
                    videoWriter.Close();
                    videoWriter.Dispose();
                }
                vmLocator.VideoPlayerVM.RaiseStopEvent();
            }
            catch (Exception ex)
            {
                String message = String.IsNullOrEmpty(ex.Message) ? "Error stopping the camera processing." : ex.Message;
                //ShowExceptionAndShutDown(message);
            }
        }

        public void StartCapture()
        {
            Boolean running = false;
            if (cameraDetector != null && cameraDetector.isRunning())
                running = true;
            FilterInfoCollection sources = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo fi in sources)
            {
                Console.WriteLine(fi.Name);
            }

            if (sources != null)
            {
                videoWriter = new VideoFileWriter();
                videoWriter.Open(@"../test_video.mp4", 640, 480, 30, VideoCodec.MPEG4);
            }

            if (!running)
                StartCapturing();
        }

        public void StartCapturing()
        {
            try
            {
                cameraDetector = new CameraDetector(0, 30, 30);
                cameraDetector.setClassifierPath(Utility.GetClassifierDataFolder());
                cameraDetector.setDetectAllExpressions(false);
                cameraDetector.setDetectAllEmotions(false);
                cameraDetector.setDetectAttention(true);
                cameraDetector.setDetectEngagement(true);
                cameraDetector.setDetectChinRaise(true);
                cameraDetector.setDetectBrowFurrow(true);
                cameraDetector.setDetectEyeClosure(true);
                cameraDetector.setDetectBrowRaise(true);
                cameraDetector.setDetectChinRaise(true);

                cameraDetector.setImageListener(this);

                cameraDetector.setLicensePath(Utility.GetAffdexLicense());
                cameraDetector.start();
                vmLocator.VideoPlayerVM.RaisePlayEvent();
            }
            catch (Affdex.AffdexException ex)
            {
                if (!String.IsNullOrEmpty(ex.Message))
                {
                    // If this is a camera failure, then reset the application to allow the user to turn on/enable camera
                    if (ex.Message.Equals("Unable to open webcam."))
                    {
                        MessageBoxResult result = MessageBox.Show(ex.Message,
                                                                "AffdexMe Error",
                                                                MessageBoxButton.OK,
                                                                MessageBoxImage.Error);
                        //StopCameraProcessing();
                        return;
                    }
                }

                String message = String.IsNullOrEmpty(ex.Message) ? "Affdex error encountered." : ex.Message;
                //ShowExceptionAndShutDown(message);
            }
            catch (Exception ex)
            {
                String message = String.IsNullOrEmpty(ex.Message) ? "AffdexMe error encountered." : ex.Message;
                //ShowExceptionAndShutDown(message);
            }
        }


        public void onImageCapture(Affdex.Frame image)
        {
            DisplayImage(image);
            UpdateExpressionsDials();
        }



        public void onImageResults(Dictionary<int, Affdex.Face> faces, Affdex.Frame image)
        {
            Affdex.Face face;
            if (faces.Count() >= 1 && faces.TryGetValue(0, out face) != false)
            {
                UpdateExpressionsDials(face);

            }
        }

        public void DisplayImage(Affdex.Frame image)
        {
            BitmapSource x = Utility.BuildImage(image.getBGRByteArray(), image.getWidth(), image.getHeight());

            //x.Freeze();
            //RenderedImage = x;                
                
            videoWriter.WriteVideoFrame(Utility.BitmapFromSource(x));

            if (image != null)
            {
                image.Dispose();
            }

            //String message = String.IsNullOrEmpty(ex.Message) ? "Error encountered while trying to display the image." : ex.Message;
            //ShowExceptionAndShutDown(message);
        }

        private void UpdateExpressionsDials(Affdex.Face face = null)
        {
            try
            {

                    if (face != null)
                    {
                    //    Console.WriteLine("chin raise" + (Convert.ToInt32(Math.Round(face.Expressions.ChinRaise, MidpointRounding.AwayFromZero))).ToString());
                    //    Console.WriteLine("attention " + (Convert.ToInt32(Math.Round(face.Expressions.Attention, MidpointRounding.AwayFromZero))).ToString());
                    //    Console.WriteLine("engagement " + (Convert.ToInt32(Math.Round(face.Emotions.Engagement, MidpointRounding.AwayFromZero))).ToString());
                    //    Console.WriteLine("brow furrow " +  (Convert.ToInt32(Math.Round(face.Expressions.BrowFurrow, MidpointRounding.AwayFromZero))).ToString());
                    //    Console.WriteLine("eye closure " +  (Convert.ToInt32(Math.Round(face.Expressions.EyeClosure, MidpointRounding.AwayFromZero))).ToString());
                    //    Console.WriteLine("brow raise" + (Convert.ToInt32(Math.Round(face.Expressions.BrowRaise, MidpointRounding.AwayFromZero))).ToString());

                        App.Current.Dispatcher.BeginInvoke((Action)(() =>
                        {
                            vmLocator.ConfusionChartVM.Collection.AddPoint(Convert.ToInt32(Math.Round(face.Expressions.BrowRaise, MidpointRounding.AwayFromZero)));
                            vmLocator.BrowFurrowChartVM.Collection.AddPoint(Convert.ToInt32(Math.Round(face.Expressions.BrowFurrow, MidpointRounding.AwayFromZero)));     
                        }));
                    }
            }
            catch (Exception ex)
            {

            }

        }
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
