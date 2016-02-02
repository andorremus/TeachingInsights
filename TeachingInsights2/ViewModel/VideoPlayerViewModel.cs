using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace TeachingInsights2.ViewModel
{
    public class VideoPlayerViewModel : ViewModelBase
    {
        private Uri mediaUri;
        private ICommand playCommand;
        public event EventHandler PlayEvent;
        private ICommand stopCommand;
        public event EventHandler StopEvent;
        private ICommand pauseCommand;
        public event EventHandler PauseEvent;

        public Uri MediaUri
        {
            get { return mediaUri; }
            set { mediaUri = value; }
        }

        private void RaisePlayEvent()
        {
            var handler = PlayEvent;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        public ICommand PlayCommand
        {
            get
            {
                if (playCommand == null)
                    playCommand = new RelayCommand(param => this.RaisePlayEvent(), null);
                return playCommand;
            }
        }
        private void RaiseStopEvent()
        {
            var handler = StopEvent;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        public ICommand StopCommand
        {
            get
            {
                if (stopCommand == null)
                    stopCommand = new RelayCommand(param => this.RaiseStopEvent(), null);
                return stopCommand;
            }
        }

        private void RaisePauseEvent()
        {
            var handler = PauseEvent;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        public ICommand PauseCommand
        {
            get
            {
                if (pauseCommand == null)
                    pauseCommand = new RelayCommand(param => this.RaisePauseEvent(), null);
                return pauseCommand;
            }
        } 

        

        public VideoPlayerViewModel()
        {
            mediaUri = new Uri("./resources/vid/affective_computing.mp4",UriKind.Relative);
            //uriPath =new Uri("C:\\projects\\TeachingInsights2\\TeachingInsights2\\resources\\vid\\affective_computing.mp4");
            //Vlc.DotNet.Wpf.VlcControl vlcPlayer = new Vlc.DotNet.Wpf.VlcControl();
            //vlcPlayer.MediaPlayer.SetMedia(uriPath);
            //vlcPlayer.MediaPlayer.Play();
            //MediaPlayer x = new MediaPlayer();
            //x.Source. = new Uri("C:\\projects\\TeachingInsights2\\TeachingInsights2\\resources\\vid\\affective_computing.mp4");
            //x.Play()

        }
    }
}
