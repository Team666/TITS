﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;
using System.ComponentModel;

namespace faceTITS
{
    public class PlaybackModel : INotifyPropertyChanged
    {
        private TITS.Components.NowPlaying _player;

        private System.Windows.Threading.DispatcherTimer _timer;

        public PlaybackModel()
        {
            if (!DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                _player = App.Player;

                _player.SongChanged += (sender, e) =>
                    {
                        NotifyPropertyChanged("CurrentArtist");
                        NotifyPropertyChanged("CurrentSongtitle");
                        NotifyPropertyChanged("MaximumSliderValue");
                    };
                _player.PlaybackStarted += (sender, e) =>
                    {
                        NotifyPropertyChanged("PlayButtonContent");
                        NotifyPropertyChanged("MaximumSliderValue");
                    };
                _player.PlaybackPaused += (sender, e) =>
                    {
                        NotifyPropertyChanged("PlayButtonContent");
                    };
                _player.PlaybackStopped += (sender, e) =>
                    {
                        NotifyPropertyChanged("PlayButtonContent");
                        NotifyPropertyChanged("MaximumSliderValue");
                    };

                _timer = new System.Windows.Threading.DispatcherTimer(System.Windows.Threading.DispatcherPriority.DataBind);
            }
        }

        public string CurrentArtist
        {
            get
            {
                if (_player.CurrentSong != null && App.Player.CurrentSong.Metadata != null)
                    return App.Player.CurrentSong.Metadata.Artist.Name;
                else
                    return "";
            }
        }

        public string CurrentSongtitle
        {
            get
            {
                if (_player.CurrentSong != null)
                {
                    if (App.Player.CurrentSong.Metadata != null)
                    {
                        return App.Player.CurrentSong.Metadata.Title;
                    }
                    else
                    {
                        return System.IO.Path.GetFileName(App.Player.CurrentSong.FileName);
                    }
                }
                else
                    return "";
            }
        }

        public string PlayButtonContent
        {
            get
            {
                switch (_player.Status)
                {
                    case TITS.Components.Engine.PlaybackStatus.Stopped:
                        return "Play";

                    case TITS.Components.Engine.PlaybackStatus.Paused:
                        return "Unpause";

                    case TITS.Components.Engine.PlaybackStatus.Playing:
                        return "Pause";

                    default:
                        return "No status";
                }
            }
        }

        public string CurrentRepeatMode
        {
            get
            {
                return _player.RepeatMode.ToString();
            }
        }

        public double CurrentSliderPosition
        {
            get
            {
                return (double)_player.Position.TotalMilliseconds;
            }
            set
            {
            }
        }

        public double MaximumSliderValue
        {
            get
            {
                _timer.Stop();
                _timer.Interval = new TimeSpan(0, 0, 1);
                _timer.Tick += TimerTickHandler;
                _timer.Start();

                if (_player.CurrentSong == null)
                    return 0;

                return (double)_player.Length.TotalMilliseconds;
            }
        }

        private void TimerTickHandler(object sender, object e)
        {
            NotifyPropertyChanged("CurrentSliderPosition");
        }


        public void NextSong()
        {
            _player.Next(forcedNext: true);
            NotifyPropertyChanged("MaximumSliderValue");
        }

        public void Play()
        {
            if (_player.Status == TITS.Components.Engine.PlaybackStatus.Stopped)
            {
                _player.StartPlaying();
                NotifyPropertyChanged("MaximumSliderValue");
            }
            else
            {
                _player.Pause();
            }

            NotifyPropertyChanged("PlayButtonContent");
        }

        public void PreviousSong()
        {
            _player.Previous();
            NotifyPropertyChanged("MaximumSliderValue");
        }

        public void CycleRepeatMode()
        {
            _player.CycleRepeatMode();

            NotifyPropertyChanged("CurrentRepeatMode");
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion
    }
}