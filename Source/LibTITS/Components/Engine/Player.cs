using System;
using System.Collections.Generic;
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

        public bool SupportsFileType(string extension)
        {
            return _supportedFileTypes.Contains(extension);
        }

        private IPlayer GetPlayer(string extension)
        {
            if (_zplayer.SupportsFileType(extension))
                return _zplayer;

            throw new FileNotSupportedException(string.Format("Extension {0} is not supported.", extension));
        }

        public void Play(Library.Song song)
        {
            _zplayer.Play(song);
        }

        public void Stop()
        {
            _zplayer.Stop();
        }

        public void Next()
        {
            _zplayer.Next();
        }
    }
}
