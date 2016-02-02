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
using AForge.Video.FFMPEG;
using System.Threading;
using TeachingInsights2.View;
using TeachingInsights2.Controls;

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
        private String currentDir;
        private VideoFileWriter videoWriter;



        #endregion
        public MainWindow()
        {
            var locator = new ViewModelLocator();
            var loginW = new LoginWindow();
            var loginVM = locator.LoginVM;

            loginVM.LoginSuccessful += (sender, args) =>
            {
                try
                {
                    loginW.DialogResult = true;
                    loginW.Close();
                }
                catch (Exception ex)
                {

                }

            };
            loginW.DataContext = loginVM;
            Nullable<bool> x = loginW.ShowDialog();
            if (x.HasValue)
                if (x.Value)
                    Console.Write(TeachingInsights2.Settings.Default.username);
                else if (!x.Value)
                    App.Current.Shutdown();
            InitializeComponent();
            webcamImgVM = new WebcamImageViewModel();
            VideoPlayerViewModel s = new VideoPlayerViewModel();

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
                        
                        videoWriter.WriteVideoFrame(Utility.BitmapFromSource(x));

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
                videoWriter = new VideoFileWriter();
                videoWriter.Open(@"../test_video.mp4", 640, 480, 30, VideoCodec.MPEG4, 10000);
            }        
            
            if (!running)
                StartCapturing();

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
                cameraDetector.start();
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
        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            StopCameraProcessing();
            if (videoWriter != null)
            {
                videoWriter.Close();
                videoWriter.Dispose();
            }
            
        }

        private void StopCameraProcessing()
        {
            try
            {
                if((cameraDetector != null) && (cameraDetector.isRunning()))
                {
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            System.Windows.Forms.OpenFileDialog theDialog = new System.Windows.Forms.OpenFileDialog();            
            theDialog.Title = "Open Text File";
            theDialog.Filter = "Video Files|*.mp4";
            theDialog.InitialDirectory = @"C:\";
            if (theDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.Windows.Forms.MessageBox.Show(theDialog.FileName.ToString());
            }
            Thread thread = new Thread(() => new Analyser().AnalyseVideo(theDialog.FileName.ToString()));
            thread.Start();            
        }

    }
}
