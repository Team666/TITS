using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TITS.Library;

namespace TITS.Components
{
    class NowPlaying
    {
        public List<Song> Playlist { get; private set; }
        private List<Song> _originalPlaylist;

        public RepeatModes RepeatMode { get; set; }

        public void ShuffleOff()
        { 

        }

        public void ShuffleAll()
        { 

        }

        public void ShuffleAlbums()
        { 

        }

        public void ShuffleArtists()
        { 

        }

    }

    public enum RepeatModes
    {
        None,
        Track,
        All
    }
}
