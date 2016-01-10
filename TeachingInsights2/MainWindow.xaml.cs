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

namespace TeachingInsights2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,Affdex.ImageListener, Affdex.ProcessStatusListener
    {
        #region private variables

        private Affdex.CameraDetector cameraDetector;


        #endregion
        public MainWindow()
        {
            InitializeComponent();
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
        }

        public void DisplayImage(Affdex.Frame image)
        {
            var result = this.Dispatcher.BeginInvoke((Action)(() =>
                {
                    try
                    {
                        CapturedImage.Source = Utility.BuildImage(image.getBGRByteArray(), image.getWidth(), image.getHeight());

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

        private String GetClassifierDataFolder()
        {
            String affdexClassifierDir = "C:\\Projects\\TeachingInsights2\\TeachingInsights2\\data\\";
            if (String.IsNullOrEmpty(affdexClassifierDir))
            {
                ShowExceptionAndShutDown("AFFDEX_DATA_DIR environment variable (Classifier Data Directory) is not set");
            }

            DirectoryInfo directoryInfo = new DirectoryInfo(affdexClassifierDir);
            if (!directoryInfo.Exists)
            {
                ShowExceptionAndShutDown("AFFDEX_DATA_DIR (Classifier Data Directory) is set to an invalid folder location");
            }

            return affdexClassifierDir;
        }

        private String GetAffdexLicense()
        {
            String licensePath = String.Empty;
            licensePath = "C:\\Projects\\TeachingInsights2\\TeachingInsights2\\license\\sdk_andor.remus@gmail.com.license";
            //if (String.IsNullOrEmpty(licensePath))
            //{
             //   ShowExceptionAndShutDown("AFFDEX_LICENSE_DIR environment variable (Affdex License Folder) is not set");
            //}

            return licensePath;
        }

        private void ShowExceptionAndShutDown(String exceptionMessage)
        {
            MessageBoxResult result = MessageBox.Show(exceptionMessage,
                                                        "AffdexMe Error",
                                                        MessageBoxButton.OK,
                                                        MessageBoxImage.Error);
            this.Dispatcher.BeginInvoke((Action)(() =>
            {
                //StopCameraProcessing();
            }));
        }

        private void startButton_click(object sender, RoutedEventArgs e)
        {
            Boolean running = false;
            if (cameraDetector != null && cameraDetector.isRunning())
                running = true;

            if (!running)
                StartCapturing();

        }

        public void StartCapturing()
        {
            try
            {
                cameraDetector = new CameraDetector();
                cameraDetector.setClassifierPath(GetClassifierDataFolder());
                cameraDetector.setDetectAllExpressions(false);
                cameraDetector.setDetectAllEmotions(false);
                cameraDetector.setDetectAttention(true);
                cameraDetector.setDetectEngagement(true);
                cameraDetector.setDetectChinRaise(true);
                
                cameraDetector.setImageListener(this);
                cameraDetector.setProcessStatusListener(this);

                cameraDetector.setLicensePath(GetAffdexLicense());
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
                        attentionValue.Text = (Convert.ToInt32(Math.Round(face.Expressions.Attention, MidpointRounding.AwayFromZero))).ToString();
                        if((face.Expressions.Attention) > 0 && face.Expressions.Attention < 34)
                            attentionValue.Background = new SolidColorBrush(Colors.Red);
                        if((face.Expressions.Attention) > 33 && face.Expressions.Attention < 67)
                            attentionValue.Background = new SolidColorBrush(Colors.Blue);
                        if((face.Expressions.Attention) > 66 && face.Expressions.Attention < 101)
                            attentionValue.Background = new SolidColorBrush(Colors.LimeGreen);


                        engagementValue.Text = (Convert.ToInt32(Math.Round(face.Emotions.Engagement, MidpointRounding.AwayFromZero))).ToString();    
                        if((face.Emotions.Engagement) > 0 && face.Emotions.Engagement < 34)
                            engagementValue.Background = new SolidColorBrush(Colors.Red);
                        if((face.Emotions.Engagement) > 33 && face.Emotions.Engagement < 67)
                            engagementValue.Background = new SolidColorBrush(Colors.Blue);
                        if((face.Emotions.Engagement) > 66 && face.Emotions.Engagement < 101)
                            engagementValue.Background = new SolidColorBrush(Colors.LimeGreen);                      
                        
                    
                    }));
            }
            catch(Exception ex)
            {

            }

        }

    }
}
