using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;
using System.ComponentModel;

namespace faceTITS
{
    public class PlaybackModel : INotifyPropertyChanged
	{
        private TITS.Components.NowPlaying _player;
        public PlaybackModel()
        {
            if (!DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
            _player = App.Player;

            _player.SongChanged += (sender, e) =>
                {
                    NotifyPropertyChanged("CurrentArtist");
                    NotifyPropertyChanged("CurrentSongtitle");
                };
            }
        }

        public string CurrentArtist
        {
            get
            {
                if (_player.CurrentSong != null)
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
                    return App.Player.CurrentSong.Metadata.Title;
                else
                    return "";
            }
        }

        public void NextSong()
        {
            _player.Next(forcedNext: true);
        }

        public void Play()
        {
            if (_player.Status == TITS.Components.Engine.PlaybackStatus.Stopped)
            {
                _player.StartPlaying();
            }
            else
            {
                _player.Pause();
            }
        }

        public void PreviousSong()
        {
            _player.Previous();
        }

        public string CurrentRepeatMode
        {
            get
            {
                return _player.RepeatMode.ToString();
            }
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