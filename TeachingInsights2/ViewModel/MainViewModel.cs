using GalaSoft.MvvmLight;
using System;
using System.ComponentModel;
using System.Windows;

namespace TeachingInsights2.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private bool isStudent;

        public bool IsStudent
        {
            get
            {
                return false;
            }
            set
            {
                isStudent = value;
                RaisePropertyChanged("IsStudent");
            }
        }

        public MainViewModel()
        {

        }
    }
}