using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
//using System.Windows.Controls;
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
using System.Threading;
using TeachingInsights2.View;
using TeachingInsights2.Controls;

namespace TeachingInsights2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        #region private variables

        #endregion
        public MainWindow()
        {
            var locator = new ViewModelLocator();
            var loginW = new LoginWindow();

            locator.LoginVM.LoginSuccessful += (sender, args) =>
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

            Nullable<bool> loginSuccessful = loginW.ShowDialog();
            if (loginSuccessful.HasValue)
                if (loginSuccessful.Value)
                {
                    UserTypeString userType = (UserTypeString)((User)App.Current.Properties["currentUser"]).userTypeId;
                    if (userType == UserTypeString.Teacher)
                    {
                        var teacherPage = new TeacherPage();
                        this.Content = teacherPage;
                    }
                    else if (userType == UserTypeString.Student)
                    {
                        var studentPage = new StudentPage();
                        this.Content = studentPage;
                    }
                }
                else if (!loginSuccessful.Value)
                    App.Current.Shutdown();
            InitializeComponent();

        }    

        //private void ShowExceptionAndShutDown(String exceptionMessage)
        //{
        //    MessageBoxResult result = MessageBox.Show(exceptionMessage,
        //                                                "AffdexMe Error",
        //                                                MessageBoxButton.OK,
        //                                                MessageBoxImage.Error);
        //    this.Dispatcher.BeginInvoke((Action)(() =>
        //    {
        //        StopCameraProcessing();
        //    }));
        //}

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
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
