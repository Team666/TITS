using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace TITS.Components.Engine
{
    class Player : IPlayer
    {
        private ZPlayer _zplayer;
        private string[] _supportedFileTypes;

        public event EventHandler<SongEventArgs> PlaybackStarted
        {
            add { _zplayer.PlaybackStarted += value; }
            remove { _zplayer.PlaybackStarted -= value; }
        }

        public event EventHandler<SongEventArgs> PlaybackPaused
        {
            add { _zplayer.PlaybackPaused += value; }
            remove { _zplayer.PlaybackPaused -= value; }
        }

        public event EventHandler<SongEventArgs> SongChanged
        {
            add { _zplayer.SongChanged += value; }
            remove { _zplayer.SongChanged -= value; }
        }

        public event EventHandler PlaybackStopped
        {
            add { _zplayer.PlaybackStopped += value; }
            remove { _zplayer.PlaybackStopped -= value; }
        }

        public event EventHandler<SongEventArgs> PlaybackError;

		public static EngineQueue QueueStatic;
        public EngineQueue Queue { get; private set; }

        public Player()
        {
            _zplayer = new ZPlayer();

            _supportedFileTypes = ZPlayer.SupportedFileTypes;
			Queue = new EngineQueue();
			QueueStatic = Queue;
        }

        public static string[] SupportedFileTypes
        {
            get { return ZPlayer.SupportedFileTypes; }
        }

        public PlaybackStatus Status
        {
            get { return _zplayer.Status; }
        }

        public TimeSpan Position
        {
            get { return _zplayer.Position; }
        }

        private IPlayer Engine { get; set; }

        public bool SupportsFileType(string extension)
        {
            return _supportedFileTypes.Contains(extension);
        }

        public void Play(Library.Song song)
        {
            Engine = GetPlayer(song);
            if (Engine != null)
                Engine.Play(song);
            else
                Trace.WriteLine(string.Format("No engine available for {0}!", song), "Warning");
        }

        public void Stop()
        {
            if (Engine != null)
                Engine.Stop();
            else
                Trace.WriteLine("No current engine available!", "Warning");
        }

        public void Pause()
        {
            if (Engine != null)
                Engine.Pause();
            else
                Trace.WriteLine("No current engine available!", "Warning");
        }

        public void Next()
        {
            Library.Song next = Queue.Current;
            Engine = GetPlayer(next);

            while (Engine == null)
            {
                // File can't be played, pick another
                next = Queue.Dequeue();
                Engine = GetPlayer(next);
            }

            if (Engine != null)
                Engine.Next();
            else
                Trace.WriteLine(string.Format("No engine available for {0}!", next), "Warning");
        }

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
