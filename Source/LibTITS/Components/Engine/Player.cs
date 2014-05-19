using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace TITS.Components.Engine
{
    /// <summary>
    /// Represents one or more playback engines.
    /// </summary>
    class Player : IPlayer
    {
        private ZPlayer _zplayer;
        private string[] _supportedFileTypes;

        /// <summary>
        /// Occurs when the engine has started or resumed playback.
        /// </summary>
        public event EventHandler<SongEventArgs> PlaybackStarted
        {
            add { _zplayer.PlaybackStarted += value; }
            remove { _zplayer.PlaybackStarted -= value; }
        }

        /// <summary>
        /// Occurs when the engine has paused playback.
        /// </summary>
        public event EventHandler<SongEventArgs> PlaybackPaused
        {
            add { _zplayer.PlaybackPaused += value; }
            remove { _zplayer.PlaybackPaused -= value; }
        }

        /// <summary>
        /// Occurs when the engine has started playing a different song.
        /// </summary>
        public event EventHandler<SongEventArgs> SongChanged
        {
            add { _zplayer.SongChanged += value; }
            remove { _zplayer.SongChanged -= value; }
        }

        /// <summary>
        /// Occurs when the engine has stopped playback.
        /// </summary>
        public event EventHandler PlaybackStopped
        {
            add { _zplayer.PlaybackStopped += value; }
            remove { _zplayer.PlaybackStopped -= value; }
        }

        /// <summary>
        /// Occurs when the volume has changed.
        /// </summary>
        public event EventHandler<VolumeEventArgs> VolumeChanged
        {
            add { _zplayer.VolumeChanged += value; }
            remove { _zplayer.VolumeChanged -= value; }
        }

        /// <summary>
        /// Occurs when a file could not be played.
        /// </summary>
        public event EventHandler<SongEventArgs> PlaybackError;

        /// <summary>
        /// Gets the queue containing songs to play.
        /// </summary>
        public EngineQueue Queue { get; private set; }

		// public Stack<Library.Song> History { get; private set; }

        /// <summary>
        /// Initializes the engine players.
        /// </summary>
        public Player()
        {
            _zplayer = new ZPlayer(this);
            Engine = _zplayer; // Default engine

            _supportedFileTypes = ZPlayer.SupportedFileTypes;
			Queue = new EngineQueue();
			// History = new Stack<Library.Song>();
        }

        /// <summary>
        /// Gets a list of file extensions that this engine supports.
        /// </summary>
        public static string[] SupportedFileTypes
        {
            get { return ZPlayer.SupportedFileTypes; }
        }

        /// <summary>
        /// Gets the current playback status.
        /// </summary>
        public PlaybackStatus Status
        {
            get { return _zplayer.Status; }
        }

        /// <summary>
        /// Gets the current playback position.
        /// </summary>
        public TimeSpan Position
        {
            get { return _zplayer.Position; }
        }

        /// <summary>
        /// Gets or sets the player volume as a value from 0 to 100.
        /// </summary>
        public int Volume
        {
            get
            {
                if (Engine != null)
                    return Engine.Volume;
                Trace.WriteLine("No current engine available!");
                return -1;
            }
            set
            {
                if (Engine != null)
                    Engine.Volume = value;
                else
                    Trace.WriteLine("No current engine available!");
            }
        }

        /// <summary>
        /// Gets or sets the particular engine that is currently being used for playback.
        /// </summary>
        private IPlayer Engine { get; set; }

        /// <summary>
        /// Determines whether an engine exists that supports the specified extension.
        /// </summary>
        /// <param name="extension">The file extension including leading period.</param>
        /// <returns>True if files with the specified extension can be played.</returns>
        public bool SupportsFileType(string extension)
        {
            return _supportedFileTypes.Contains(extension);
        }

        /// <summary>
        /// Starts playback with the specified song.
        /// </summary>
        /// <param name="song">The song to be played.</param>
        public void Play(Library.Song song)
        {
            Engine = GetPlayer(song);
            if (Engine != null)
                Engine.Play(song);
            else
                Trace.WriteLine(string.Format("No engine available for {0}!", song), "Warning");
        }

        /// <summary>
        /// Stops playback.
        /// </summary>
        public void Stop()
        {
            if (Engine != null)
                Engine.Stop();
            else
                Trace.WriteLine("No current engine available!", "Warning");
        }

        /// <summary>
        /// Pauses or resumes playback.
        /// </summary>
        public void Pause()
        {
            if (Engine != null)
                Engine.Pause();
            else
                Trace.WriteLine("No current engine available!", "Warning");
        }

        /// <summary>
        /// Skips the current song and plays the next song. If the next song is
        /// not supported by any engine, the song after that is played instead.
        /// </summary>
        public void Next()
        {
            if (Queue.Count > 0)
            {
                Library.Song next = Queue.Dequeue();
                ChangeSong(next);
            }
            else
            {
                Debug.WriteLine("Queue is empty");
            }
        }

		public void Previous()
		{
			throw new NotImplementedException();

			Library.Song previous = null;
            ChangeSong(previous);
		}

        public void ChangeSong(Library.Song song)
        {
            Engine = GetPlayer(song);
            if (Engine != null)
                Engine.ChangeSong(song);
            else
                Trace.WriteLine(string.Format("No engine available for {0}!", song), "Warning");
        }

        public Library.Song CurrentSong
        {
            get
            {
                if (Engine != null)
                {
                    return Engine.CurrentSong;
                }

                Trace.WriteLine("No current engine available!", "Warning");
                return null;
            }
        }

        /// <summary>
        /// Determines which engine can be used to play the specified song. If
        /// it is not supported by any engine, returns null.
        /// </summary>
        /// <param name="song">The song for which to find an engine.</param>
        /// <returns>The engine to be used for playback, or null.</returns>
        private IPlayer GetPlayer(Library.Song song)
        {
            string extension = Path.GetExtension(song.FileName);
            if (_zplayer.SupportsFileType(extension))
                return _zplayer;

            Trace.WriteLine(string.Format("Extension {0} is not supported.", extension), "Warning");
            if (PlaybackError != null) PlaybackError(this, new SongEventArgs(song));
            return null;
        }
	}
}
