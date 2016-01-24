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
using Vlc.DotNet.Wpf;

namespace TeachingInsights2.controls
{
    /// <summary>
    /// Interaction logic for WebcamFeed.xaml
    /// </summary>
    public partial class WebcamFeed : UserControl
    {
        public WebcamFeed()
        {
            InitializeComponent();
            
        }

        private void Image_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            Console.WriteLine("updated");
        }
    }
}
