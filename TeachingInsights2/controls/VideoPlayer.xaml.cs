﻿using System;
using System.Collections.Generic;
using System.IO;
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
using TeachingInsights2.ViewModel;
using Vlc.DotNet.Wpf;

namespace TeachingInsights2.Controls
{
    /// <summary>
    /// Interaction logic for VideoPlayer.xaml
    /// </summary>
    public partial class VideoPlayer : UserControl
    {
        public VideoPlayer()
        {
            InitializeComponent();
            var locator = new ViewModelLocator();
            var videoVM = locator.VideoPlayerVM;

            videoVM.PlayEvent += (sender, args) =>
            {
                mediaPlayer.Play();
            };
            videoVM.StopEvent += (sender, args) =>
            {
                mediaPlayer.Stop();
            };
            videoVM.PauseEvent += (sender, args) =>
            {
                mediaPlayer.Pause();
            };


        }
    }
}
