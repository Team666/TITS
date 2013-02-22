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

		public NowPlaying()
		{
			_player.Queue.QueueEmpty += this.EnqueueNextSong;
		}

        public void EnqueueNextSong(object sender, EventArgs e)
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
