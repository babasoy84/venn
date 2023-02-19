using CloudinaryDotNet.Actions;
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
using System.Windows.Threading;

namespace Venn.Client.MVVM.Views
{
    /// <summary>
    /// Interaction logic for ChatView.xaml
    /// </summary>
    public partial class ChatView : UserControl
    {
        private bool isPlaying = true;

        private DispatcherTimer timerVideoPlayback;

        public ChatView()
        {
            InitializeComponent();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (isPlaying)
                Player.Play();
            else
                Player.Pause();

            isPlaying = !isPlaying;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isPlaying)
            {
                _Player.Play();
                if (timerVideoPlayback != null)
                {
                    timerVideoPlayback.Start();
                }
            }
            else
            {
                _Player.Pause();
                if (timerVideoPlayback != null)
                {
                    timerVideoPlayback.Stop();
                }
            }

            isPlaying = !isPlaying;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_Player.NaturalDuration.TimeSpan.TotalSeconds > 0)
            {
                if (_Player.Position.TotalSeconds - TimelineSlider.Value < -1 || _Player.Position.TotalSeconds - TimelineSlider.Value > 1)
                {
                    _Player.Position = TimeSpan.FromSeconds(TimelineSlider.Value);
                }
            }
        }

        private void _Player_MediaOpened(object sender, RoutedEventArgs e)
        {
            TimelineSlider.Maximum = _Player.NaturalDuration.TimeSpan.TotalSeconds;


            timerVideoPlayback = new DispatcherTimer();
            timerVideoPlayback.Interval = TimeSpan.FromMilliseconds(10);
            timerVideoPlayback.Tick += TimerVideoPlayback_Tick;
            timerVideoPlayback.Start();
        }

        private void _Player_MediaEnded(object sender, RoutedEventArgs e)
        {
            _Player.Stop();
            timerVideoPlayback.Stop();
            isPlaying = false;
        }

        private void TimerVideoPlayback_Tick(object sender, object e)
        {
            if (_Player.NaturalDuration.HasTimeSpan)
            {
                if (_Player.NaturalDuration.TimeSpan.TotalSeconds > 0)
                {
                    // Updating time slider
                    TimelineSlider.Value = _Player.Position.TotalSeconds;
                }
            }
        }
    }
}
