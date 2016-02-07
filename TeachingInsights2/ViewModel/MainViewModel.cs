using GalaSoft.MvvmLight;
using System;
using System.ComponentModel;
using System.Windows;

namespace TeachingInsights2.ViewModel
{
    public class MainViewModel : DependencyObject
    {
        // Using a DependencyProperty as the backing store for MediaUri.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MediaUriProperty =
            DependencyProperty.Register("MediaUri", typeof(Uri), typeof(MainViewModel), new PropertyMetadata("./resources/vid/affective_computing.mp4"));
        public Uri MediaUri
        {
            get { return (Uri)GetValue(MediaUriProperty); }
            set { SetValue(MediaUriProperty, value); }
        }


        
        public MainViewModel()
        {
           MediaUri = new Uri("./resources/vid/affective_computing.mp4", UriKind.Relative);
        }


    }
}