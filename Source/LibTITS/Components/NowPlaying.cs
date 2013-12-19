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

        public event EventHandler<Engine.SongEventArgs> PlaybackStarted
        {
            add { _player.PlaybackStarted += value; }
            remove { _player.PlaybackStarted -= value; }
        }

        public event EventHandler<Engine.SongEventArgs> PlaybackPaused
        {
            add { _player.PlaybackPaused += value; }
            remove { _player.PlaybackPaused -= value; }
        }

        public event EventHandler<Engine.SongEventArgs> SongChanged
        {
            add { _player.SongChanged += value; }
            remove { _player.SongChanged -= value; }
        }

        public event EventHandler<Engine.SongEventArgs> PlaybackError
        {
            add { _player.PlaybackError += value; }
            remove { _player.PlaybackError -= value; }
        }

        public event EventHandler PlaybackStopped
        {
            add { _player.PlaybackStopped += value; }
            remove { _player.PlaybackStopped -= value; }
        }


        /// <summary>
        /// Initializes a new instance of the NowPlaying class.
        /// </summary>
		public NowPlaying()
		{
            _player = new Engine.Player();
            _player.Queue.QueueEmpty += (sender, e) => { this.EnqueueNextSong(); };
		}

        /// <summary>
        /// Gets or sets the currently playing playlist.
        /// </summary>
        public Playlist Playlist { get; set; }

        /// <summary>
        /// Gets or sets the current repeat mode.
        /// </summary>
        public RepeatModes RepeatMode { get; set; }

        /// <summary>
        /// Gets the current playback status.
        /// </summary>
        public Engine.PlaybackStatus Status
        {
            get { return _player.Status; }
        }

        /// <summary>
        /// Gets the position of the currently playing song.
        /// </summary>
        public TimeSpan Position
        {
            get { return _player.Position; }
        }

        /// <summary>
        /// Gets whether music is currently playing or not.
        /// </summary>
        public bool IsPlaying
        {
            get { return _player.Status == Engine.PlaybackStatus.Playing; }
        }

        /// <summary>
        /// Starts playback.
        /// </summary>
		public void StartPlaying()
		{
			EnqueueNextSong();
			_player.Play(_player.Queue.Dequeue());
		}

        /// <summary>
        /// Stops playback.
        /// </summary>
        public void Stop()
        {
            _player.Stop();
        }

        /// <summary>
        /// Pauses or resumes playback.
        /// </summary>
        public void Pause()
        {
            _player.Pause();
        }

        /// <summary>
        /// Starts playing the next song.
        /// </summary>
        public void Next()
        {
            if (Status == Engine.PlaybackStatus.Stopped)
                StartPlaying();
            else
                _player.Next();
        }

        /// <summary>
        /// Enqueues the next song for playback, and updates the playlist index.
        /// </summary>
        private void EnqueueNextSong()
        {
            _player.Queue.Enqueue(Playlist.NextSong);
            Playlist.Index++;
        }
    }

    public enum RepeatModes
    {
        None,
        Track,
        All
    }
}
