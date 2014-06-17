using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace faceTITS.ViewModels
{
    class NowPlayingPlaylistModel : INotifyPropertyChanged
    {
        private TITS.Components.NowPlaying _player;        

        public int SongIndex
        {
            get
            {
                if (_player.Playlist == null)
                {
                    return -1;
                }

                return _player.Playlist.Index;
            }
        }

        public List<TITS.Library.Song> Playlist
        {
            get
            {
                if (_player.Playlist == null)
                {
                    return TITS.Library.Playlist.Empty;
                }
                else
                {
                    return _player.Playlist;
                }
            }
        }

        public NowPlayingPlaylistModel()
        {
            if (!DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                _player = App.Player;
                HookupHandlers();
            }
        }

        public void HookupHandlers()
        {
            _player.PlaylistChanged += (sender, e) =>
            {
                NotifyPropertyChanged("Playlist");

                // When playlist changes, hookup ProperyChanged for its Index value
                _player.Playlist.PropertyChanged += (innerSender, innerE) =>
                {
                    if (innerE.PropertyName == "Index")
                    {
                        NotifyPropertyChanged("SongIndex");
                    }
                };
            };
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
