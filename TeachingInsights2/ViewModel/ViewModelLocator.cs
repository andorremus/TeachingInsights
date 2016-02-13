/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:TeachingInsights2"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace TeachingInsights2.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<WebcamFeedViewModel>();
            SimpleIoc.Default.Register<VideoPlayerViewModel>();
            SimpleIoc.Default.Register<TCPClientViewModel>();
            SimpleIoc.Default.Register<TCPServerViewModel>();
            SimpleIoc.Default.Register<UserAccountViewModel>();
            SimpleIoc.Default.Register<StudentPageViewModel>();
            SimpleIoc.Default.Register<ConfusionChartViewModel>();
            SimpleIoc.Default.Register<BrowFurrowChartViewModel>();
            SimpleIoc.Default.Register<TeacherPageViewModel>();
        }

        public MainViewModel MainVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public WebcamFeedViewModel WebcamFeedVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<WebcamFeedViewModel>();
            }
        }

        public LoginViewModel LoginVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LoginViewModel>();
            }
        }

        public VideoPlayerViewModel VideoPlayerVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<VideoPlayerViewModel>();
            }
        }

        public TCPClientViewModel TCPClientVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<TCPClientViewModel>();
            }
        }

        public TCPServerViewModel TCPServerVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<TCPServerViewModel>();
            }
        }
        public UserAccountViewModel UAVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<UserAccountViewModel>();
            }
        }
        public ConfusionChartViewModel ConfusionChartVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ConfusionChartViewModel>();
            }
        }

        public StudentPageViewModel StudentPageVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<StudentPageViewModel>();
            }
        }

        public TeacherPageViewModel TeacherPageVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<TeacherPageViewModel>();
            }
        }

        public BrowFurrowChartViewModel BrowFurrowChartVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<BrowFurrowChartViewModel>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}