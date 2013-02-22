using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TITS.Library;

namespace TITS.Components
{
    class NowPlaying
    {
        private Engine.Player _player;

        public Playlist Playlist { get; private set; }

        public RepeatModes RepeatMode { get; set; }

        public void EnqueueNextSong()
        {
            _player.Queue.Enqueue(Playlist.NextSong);
        }
    }

    public enum RepeatModes
    {
        None,
        Track,
        All
    }
}
