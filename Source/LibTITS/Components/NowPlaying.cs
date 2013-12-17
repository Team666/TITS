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

        /// <summary>
        /// Gets or sets the currently playing playlist.
        /// </summary>
        public Playlist Playlist { get; set; }

        /// <summary>
        /// Gets or sets the current repeat mode.
        /// </summary>
        public RepeatModes RepeatMode { get; set; }

        /// <summary>
        /// Initializes a new instance of the NowPlaying class.
        /// </summary>
		public NowPlaying()
		{
            _player = new Engine.Player();
            _player.Queue.QueueEmpty += (sender, e) => { this.EnqueueNextSong(); };
		}

        /// <summary>
        /// Enqueues the next song for playback, and updates the playlist index.
        /// </summary>
        public void EnqueueNextSong()
        {
            _player.Queue.Enqueue(Playlist.NextSong);
            Playlist.Index++;
        }

        /// <summary>
        /// Starts playback.
        /// </summary>
		public void StartPlaying()
		{
			EnqueueNextSong();
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
