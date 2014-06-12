using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;
using System.ComponentModel;

namespace faceTITS
{
    public class View1Model : INotifyPropertyChanged
	{
        public View1Model()
        {
            App.Player.SongChanged += (sender, e) =>
                {
                    NotifyPropertyChanged("CurrentArtist");
                    NotifyPropertyChanged("CurrentSongtitle");
                };

        }

        public string CurrentArtist
        {
            get
            {
                if (App.Player.CurrentSong != null)
                    return App.Player.CurrentSong.Metadata.Artist.Name;
                else
                    return "";
            }
        }

        public string CurrentSongtitle
        {
            get
            {
                if (App.Player.CurrentSong != null)
                    return App.Player.CurrentSong.Metadata.Title;
                else
                    return "";
            }
        }

        public static void NextSong()
        {
            App.Player.Next(forcedNext: true);
        }

        public static void Play()
        {
            if (App.Player.Status == TITS.Components.Engine.PlaybackStatus.Stopped)
            {
                App.Player.StartPlaying();
            }
            else
            {
                App.Player.Pause();
            }
        }

        public static void PreviousSong()
        {
            App.Player.Previous();
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