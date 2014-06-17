using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using TITS.Library;
using System.ComponentModel;

namespace TITS.Components
{
    public class NowPlaying
    {
        private Engine.Player _player;

        /// <summary>
        /// Occurs when the engine has started or resumed playback.
        /// </summary>
        public event EventHandler<Engine.SongEventArgs> PlaybackStarted
        {
            add { _player.PlaybackStarted += value; }
            remove { _player.PlaybackStarted -= value; }
        }

        /// <summary>
        /// Occurs when the engine has paused playback.
        /// </summary>
        public event EventHandler<Engine.SongEventArgs> PlaybackPaused
        {
            add { _player.PlaybackPaused += value; }
            remove { _player.PlaybackPaused -= value; }
        }

        /// <summary>
        /// Occurs when the engine has started playing a different song.
        /// </summary>
        public event EventHandler<Engine.SongEventArgs> SongChanged
        {
            add { _player.SongChanged += value; }
            remove { _player.SongChanged -= value; }
        }

        /// <summary>
        /// Occurs when the engine has stopped playback.
        /// </summary>
        public event EventHandler PlaybackStopped
        {
            add { _player.PlaybackStopped += value; }
            remove { _player.PlaybackStopped -= value; }
        }

        /// <summary>
        /// Occurs when a file could not be played.
        /// </summary>
        public event EventHandler<Engine.SongEventArgs> PlaybackError
        {
            add { _player.PlaybackError += value; }
            remove { _player.PlaybackError -= value; }
        }

        /// <summary>
        /// Occurs when the volume has changed.
        /// </summary>
        public event EventHandler<Engine.VolumeEventArgs> VolumeChanged
        {
            add { _player.VolumeChanged += value; }
            remove { _player.VolumeChanged -= value; }
        }

        /// <summary>
        /// Initializes a new instance of the NowPlaying class.
        /// </summary>
        public NowPlaying()
        {
            _player = new Engine.Player(this.NotifyPlaylistOfDequeue);
            _player.Queue.QueueEmpty += (sender, e) => { this.EnqueueNextSong(); };
        }

        /// <summary>
        /// Gets or sets the currently playing playlist.
        /// </summary>
        private Playlist _playlist;
        public Playlist Playlist 
        { 
            get
            {
                return _playlist;
            }
            set
            {
                // Have to also give the new Playlist the current repeatMode
                _playlist = value;
                _playlist.RepeatMode = _repeatMode;
                _playlist.CollectionChanged += PlaylistChanged;
                onPlaylistChanged(_playlist);
            }
        }

        /// <summary>
        /// Notifies playlist of automatic song change (next song) by calling its OffsetIndexBy method
        /// this is needed for EngineQueue once it proceeds to play the queued song.
        /// </summary>
        private void NotifyPlaylistOfDequeue()
        {
            if (RepeatMode != RepeatModes.Track)
            {
                _playlist.OffsetIndexBy(+1);
            }
        }

        /// <summary>
        /// Gets or sets the current repeat mode.
        /// </summary>
        private RepeatModes _repeatMode;
        public RepeatModes RepeatMode 
        { 
            get
            {
                return _repeatMode;
            }
            set
            {
                _repeatMode = value;
                Playlist.RepeatMode = value;

                if (_repeatMode == RepeatModes.Track)
                {
                    _player.Queue.Flush();
                }
            }
        }

        public void CycleRepeatMode()
        {
            switch (_repeatMode)
            {
                case RepeatModes.None:
                    RepeatMode = RepeatModes.All;
                    break;

                case RepeatModes.All:
                    RepeatMode = RepeatModes.Track;
                    break;

                case RepeatModes.Track:
                    RepeatMode = RepeatModes.None;
                    break;
            }
        }

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
        /// Gets the current song's Length
        /// </summary>
        public TimeSpan Length
        {
            get { return _player.Length; }
        }

        /// <summary>
        /// Gets or sets the player volume as a value from 0 to 100.
        /// </summary>
        public int Volume
        {
            get { return _player.Volume; }
            set
            {
                if (value > 100) value = 100;
                if (value < 0) value = 0;
                _player.Volume = value;
            }
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
            if (Playlist.Count > 0)
            {
                EnqueueNextSong();
                _player.Play(_player.Queue.Dequeue());
            }
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
        /// Plays the next song.
        /// </summary>
        public void Next(bool forcedNext = true)
        {
            var song = Playlist.NextSong(peek: false, forcedNext: forcedNext);

            ChangeSong(song);
        }

		public void Previous()
		{
            var song = Playlist.PreviousSong(peek: false);

            ChangeSong(song);
		}

        public void ChangeSong(Song song)
        {
            if (song != null)
            {
                _player.Queue.Flush();
                _player.ChangeSong(song);
            }
        }

        /// <summary>
        /// Enqueues the next song for playback
        /// </summary>
        private void EnqueueNextSong()
        {
            if (Playlist.Index + 1 >= Playlist.Count && RepeatMode == RepeatModes.None)
            {
                Trace.WriteLine("End of playlist");
                return;
            }

            Song next;
            if (Playlist.Index < 0 || RepeatMode != RepeatModes.Track)
            {
                next = Playlist.NextSong(peek: true);
            }
            else // if (RepeatMode == RepeatModes.Track)
            {
                next = Playlist.CurrentSong;
            }

            if (next != null)
            {
                _player.Queue.Enqueue(next);
            }
            else
            {
                return;
            }
        }

        public Library.Song CurrentSong
        {
            get
            {
                return _player.CurrentSong;
            }
        }

        public event System.ComponentModel.CollectionChangeEventHandler PlaylistChanged;

        private void onPlaylistChanged(object sender)
        {
            if (this.PlaylistChanged != null)
            {
                this.PlaylistChanged(sender, new CollectionChangeEventArgs(CollectionChangeAction.Refresh, "NowPlaying"));
            }
        }
    }

    public enum RepeatModes
    {
        None,
        Track,
        All
    }
}
