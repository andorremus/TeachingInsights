using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Affdex;
using System.IO;
using System.Reflection;
using System.Collections.Specialized;
using Sparrow.Chart;

using System.Collections.ObjectModel;
using TeachingInsights2.ViewModel;
using AForge.Video.DirectShow;
using AForge.Video;
using System.Drawing;
using System.Drawing.Imaging;
using DotImaging;

namespace TeachingInsights2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,Affdex.ImageListener, Affdex.ProcessStatusListener
    {
       
        #region private variables
        private WebcamImageViewModel webcamImgVM;
        private Affdex.CameraDetector cameraDetector;
        private int counter = 0;
        private VideoCaptureDevice videoSource;        
        private String currentDir;
        private String tempImgDir;
        private ImageStreamWriter writer;



        #endregion
        public MainWindow()
        {
            InitializeComponent();
            webcamImgVM = new WebcamImageViewModel();
            currentDir = Environment.CurrentDirectory;
        }


        public void onImageResults(Dictionary<int, Affdex.Face> faces, Affdex.Frame image)
        {
            if(faces.Count() >= 1)
            {
                Affdex.Face face = faces[0];
                UpdateExpressionsDials(face);
                var result = this.Dispatcher.BeginInvoke((Action)(() =>                
                {
                    //this.textBox.Text = face.Expressions.Attention.ToString();                   
                }));
                
            }
        }
        public void onImageCapture(Affdex.Frame image)
        {
            DisplayImage(image);
            //Image<Bgr<byte>> imagex;
            
            //writer.Write((IImage)imagex);

            //UpdateExpressionsDials();
        }

        public void DisplayImage(Affdex.Frame image)
        {
            var result = this.Dispatcher.BeginInvoke((Action)(() =>
                {
                    try
                    {
                        BitmapSource x = Utility.BuildImage(image.getBGRByteArray(), image.getWidth(), image.getHeight());
                        webcamImgVM.RenderedImage = x;
                        
                        //videoWriter.WriteVideoFrame(Utility.BitmapFromSource(x));

                        if (image != null)
                        {
                            image.Dispose();
                        }
                    }
                    catch(Exception ex)
                    {
                        String message = String.IsNullOrEmpty(ex.Message) ? "Error encountered while trying to display the image." : ex.Message;
                        ShowExceptionAndShutDown(message);
                    }

                }));
            //BitmapSource xz = Utility.BuildImage(image.getBGRByteArray(), image.getWidth(), image.getHeight());
            //Bitmap x3 = Utility.BitmapFromSource(xz);
            //x3.Save(currentDir + "/tempImg/img" + counter + ".jpeg", ImageFormat.Jpeg);
            //counter++;

        }

      

        public void onProcessingException(Affdex.AffdexException affdexExc)
        {
            String message = String.IsNullOrEmpty(affdexExc.Message) ? "AffdexMe error encountered." : affdexExc.Message;
            MessageBoxResult result = MessageBox.Show(message,
                                                       "AffdexMe Error",
                                                       MessageBoxButton.OK,
                                                       MessageBoxImage.Error);
        }

        public void onProcessingFinished()
        {

        }

        private void ShowExceptionAndShutDown(String exceptionMessage)
        {
            MessageBoxResult result = MessageBox.Show(exceptionMessage,
                                                        "AffdexMe Error",
                                                        MessageBoxButton.OK,
                                                        MessageBoxImage.Error);
            this.Dispatcher.BeginInvoke((Action)(() =>
            {
                StopCameraProcessing();
            }));
        }

        private void startButton_click(object sender, RoutedEventArgs e)
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
                videoSource = new VideoCaptureDevice(sources[0].MonikerString);

                try
                {
                    if(videoSource.VideoCapabilities.Length > 0)
                    {
                        string highestSolution = "0;0";

                        for(int i = 0 ; i< videoSource.VideoCapabilities.Length; i++)
                        {
                            highestSolution = videoSource.VideoCapabilities[i].FrameSize.Width.ToString() + ";" + i.ToString();
                        }
                        videoSource.VideoResolution = videoSource.VideoCapabilities[Convert.ToInt32(highestSolution.Split(';')[1])];
                    }
                }
                catch
                {

                }
                //videoWriter = new VideoFileWriter();
                //videoWriter.Open(@"../test_video.mp4", 640, 480, 25, VideoCodec.MPEG4, 10000);

                
                videoSource.NewFrame += new AForge.Video.NewFrameEventHandler(VideoSource_NewFrame);
                videoSource.Start();

            }
            
            System.IO.Directory.CreateDirectory("tempImg");
            //writer = new VideoWriter(
            //                                 fileName: "./output.avi",
            //                                 frameSize: new DotImaging.Primitives2D.Size(1280, 720),
            //                                 fps: 30
            //                               );          
            
            if (!running)
                StartCapturing();

        }

        void VideoSource_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            this.Dispatcher.BeginInvoke((Action)(() =>
            {
                //image.Source = (ImageSource)eventArgs.Frame.Clone();
                
            }));
            using (Bitmap img = (Bitmap)eventArgs.Frame.Clone())
            {
                img.Save(currentDir+"/tempImg/img" + counter+".jpeg", ImageFormat.Jpeg);
                counter++;
            }
        }

        public void StartCapturing()
        {
            try
            {
                cameraDetector = new CameraDetector(0,30,30);
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
                //cameraDetector.start();
                //writer.Open();  
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
                ShowExceptionAndShutDown(message);
            }
            catch (Exception ex)
            {
                String message = String.IsNullOrEmpty(ex.Message) ? "AffdexMe error encountered." : ex.Message;
                ShowExceptionAndShutDown(message);
            }
        }

        private FileInfo[] files;
        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            StopCameraProcessing();
            videoSource.SignalToStop();
            videoSource.Stop();
            //videoWriter.Close();
            
        }

        private void StopCameraProcessing()
        {
            try
            {
                if((cameraDetector != null) && (cameraDetector.isRunning()))
                {
                    //writer.Close();
                   // writer.Dispose();
                    cameraDetector.stop();
                    cameraDetector.Dispose();
                    cameraDetector = null;
                    
                }
            }
            catch(Exception ex)
            {
                String message = String.IsNullOrEmpty(ex.Message) ? "Error stopping the camera processing." : ex.Message;
                ShowExceptionAndShutDown(message);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            StopCameraProcessing();
            Application.Current.Shutdown();
        }

        //private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    try 
        //    {                
        //        double x = (Convert.ToDouble(this.textBox.Text));
        //        if(x < 33 )
        //            this.textBox.Background = new SolidColorBrush(Colors.Red);
        //        else if(33 <= x && x < 66)
        //            this.textBox.Background = new SolidColorBrush(Colors.Blue);
        //        else if(66 <= x && x <= 100)
        //                this.textBox.Background = new SolidColorBrush(Colors.Green);
        //        else
        //            this.textBox.Background = new SolidColorBrush(Colors.Transparent);
        //    }
        //    catch(Exception ex)
        //    {
        //        String message = String.IsNullOrEmpty(ex.Message) ? "Error changing the background color." : ex.Message;
        //        ShowExceptionAndShutDown(message);
        //    }
        //}

        private void UpdateExpressionsDials(Affdex.Face face = null)
        {
            try
            {
                var result = this.Dispatcher.BeginInvoke((Action)(() =>
                    {

                        

                        if (face != null)
                        {
                            chinRaiseValue.Text = (Convert.ToInt32(Math.Round(face.Expressions.ChinRaise, MidpointRounding.AwayFromZero))).ToString();
                            attentionValue.Text = (Convert.ToInt32(Math.Round(face.Expressions.Attention, MidpointRounding.AwayFromZero))).ToString();
                            engagementValue.Text = (Convert.ToInt32(Math.Round(face.Emotions.Engagement, MidpointRounding.AwayFromZero))).ToString();
                            browFurrowValue.Text = (Convert.ToInt32(Math.Round(face.Expressions.BrowFurrow, MidpointRounding.AwayFromZero))).ToString();
                            eyeClosureValue.Text = (Convert.ToInt32(Math.Round(face.Expressions.EyeClosure, MidpointRounding.AwayFromZero))).ToString();
                            browRaiseValue.Text = (Convert.ToInt32(Math.Round(face.Expressions.BrowRaise, MidpointRounding.AwayFromZero))).ToString();

                            faceDetectedEllipse.Fill = new SolidColorBrush(Colors.Green);
                            if ((face.Expressions.Attention) > 0 && face.Expressions.Attention < 34)
                                attentionValue.Background = new SolidColorBrush(Colors.Red);
                            if ((face.Expressions.Attention) > 33 && face.Expressions.Attention < 67)
                                attentionValue.Background = new SolidColorBrush(Colors.LightBlue);
                            if ((face.Expressions.Attention) > 66 && face.Expressions.Attention < 101)
                                attentionValue.Background = new SolidColorBrush(Colors.LimeGreen);


                            if ((face.Emotions.Engagement) > 0 && face.Emotions.Engagement < 34)
                                engagementValue.Background = new SolidColorBrush(Colors.Red);
                            if ((face.Emotions.Engagement) > 33 && face.Emotions.Engagement < 67)
                                engagementValue.Background = new SolidColorBrush(Colors.LightBlue);
                            if ((face.Emotions.Engagement) > 66 && face.Emotions.Engagement < 101)
                                engagementValue.Background = new SolidColorBrush(Colors.LimeGreen);

                            if ((face.Expressions.BrowFurrow) > 0 && face.Expressions.BrowFurrow < 34)
                                browFurrowValue.Background = new SolidColorBrush(Colors.Red);
                            if ((face.Expressions.BrowFurrow) > 33 && face.Expressions.BrowFurrow < 67)
                                browFurrowValue.Background = new SolidColorBrush(Colors.LightBlue);
                            if ((face.Expressions.BrowFurrow) > 66 && face.Expressions.BrowFurrow < 101)
                                browFurrowValue.Background = new SolidColorBrush(Colors.LimeGreen);

                            if ((face.Expressions.EyeClosure) > 0 && face.Expressions.EyeClosure < 34)
                                eyeClosureValue.Background = new SolidColorBrush(Colors.Red);
                            if ((face.Expressions.EyeClosure) > 33 && face.Expressions.EyeClosure < 67)
                                eyeClosureValue.Background = new SolidColorBrush(Colors.LightBlue);
                            if ((face.Expressions.EyeClosure) > 66 && face.Expressions.EyeClosure < 101)
                                eyeClosureValue.Background = new SolidColorBrush(Colors.LimeGreen);

                            
                            if ((face.Expressions.BrowRaise) > 0 && face.Expressions.BrowRaise < 34)
                                browRaiseValue.Background = new SolidColorBrush(Colors.Red);
                            if ((face.Expressions.BrowRaise) > 33 && face.Expressions.BrowRaise < 67)
                                browRaiseValue.Background = new SolidColorBrush(Colors.LightBlue);
                            if ((face.Expressions.BrowRaise) > 66 && face.Expressions.BrowRaise < 101)
                                browRaiseValue.Background = new SolidColorBrush(Colors.LimeGreen);

                            if ((face.Expressions.ChinRaise) > 0 && face.Expressions.ChinRaise < 34)
                                chinRaiseValue.Background = new SolidColorBrush(Colors.Red);
                            if ((face.Expressions.ChinRaise) > 33 && face.Expressions.ChinRaise < 67)
                                chinRaiseValue.Background = new SolidColorBrush(Colors.LightBlue);
                            if ((face.Expressions.ChinRaise) > 66 && face.Expressions.ChinRaise < 101)
                                chinRaiseValue.Background = new SolidColorBrush(Colors.LimeGreen);



                            //ViewModel.Collection.Add(new ViewModel.Point { X = counter, Y = face.Expressions.Attention });
                            //counter += 0.0001;
                            //DataViewModel.AddFurrow(new DataViewModel.Point { X = counter, Y = face.Expressions.BrowFurrow });
                            //DataViewModel.Add(new DataViewModel.Point { X = counter, Y = face.Expressions.BrowRaise });
                            //counter++;
                        }

                        
                    }));
                    

            }
            catch(Exception ex)
            {

            }

        }

        private void makeAvi()
        {   // reads all images in folder 
            ImageStreamReader reader = new ImageDirectoryCapture(currentDir+"/tempImg/", "*.jpeg", recursive: false);
            reader.Open();
            writer = new VideoWriter(
                                            fileName: "./output.avi",
                                            frameSize: new DotImaging.Primitives2D.Size(1280, 720),
                                            fps: 30
                                          );
            writer.Open();

            var singleImg = reader.ReadAs<Bgr<byte>>();

            foreach(IImage img in reader)
            {                
                writer.Write(img); 
            }
            reader.Close();
            writer.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(new Analyser().AnalyseVideo));
            thread.Start();

            //foreach(FileInfo file in files)
            //{
            //    try
            //    {
            //        file.Delete();
            //    }
            //    catch(Exception exx)
            //    {

            //    }
            //}
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(currentDir + "/tempImg/");
            files = dirInfo.GetFiles().OrderBy(p => p.CreationTime).ToArray();
            int f = new Random().Next(0, files.Length);
            ImageSource imageToProcess = (new ImageSourceConverter()).ConvertFromString(currentDir + "/tempImg/img" + f + ".jpeg") as ImageSource;
            image.Source = imageToProcess;
            //Console.WriteLine(f);

            System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(makeAvi));
            thread.Start();
        }

    }
}
