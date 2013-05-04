using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TITS.Library;

namespace TITS.Components
{
    public class NowPlaying
    {
        private Engine.Player _player;

        public Playlist Playlist { get; set; }

        public RepeatModes RepeatMode { get; set; }

		public NowPlaying()
		{
            _player = new Engine.Player();
			_player.Queue.QueueEmpty += this.EnqueueNextSong;
		}

        public void EnqueueNextSong(object sender, EventArgs e)
        {
            _player.Queue.Enqueue(Playlist.NextSong);
        }

		public void StartPlaying()
		{
			EnqueueNextSong(this, EventArgs.Empty);
			_player.Play(_player.Queue.Dequeue());
		}
    }

    public enum RepeatModes
    {
        None,
        Track,
        All
    }
}
