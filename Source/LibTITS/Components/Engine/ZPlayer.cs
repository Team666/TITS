using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TITS.Library;
using libZPlay;

namespace TITS.Components.Engine
{
    class ZPlayer : IPlayer
    {
        private Song _currentSong;
        private static ZPlay _engine = null;
		private System.Threading.Thread _thread;
        private static string[] _supportedFileTypes = { ".mp3", ".mp2", ".mp1", ".ogg", ".flac", ".oga", ".aac", ".wav" };

        public event EventHandler<SongEventArgs> PlaybackStarted;
        public event EventHandler<SongEventArgs> SongChanged;
        public event EventHandler PlaybackStopped;

        public static string[] SupportedFileTypes
        {
            get { return _supportedFileTypes; }
        }

        public PlaybackStatus Status
        {
            get
            {
                TStreamStatus status = default(TStreamStatus);
                Engine.GetStatus(ref status);

                if (status.fPlay)
                    return PlaybackStatus.Playing;
                else
                    return PlaybackStatus.Stopped;
            }
        }

        public bool SupportsFileType(string extension)
        {
            return SupportedFileTypes.Contains(extension);
        }

        internal static ZPlay Engine
        {
            get
            {
                if (_engine == null)
                    _engine = InitializeEngine();
                return _engine;
            }
            private set
            {
                _engine = value;
            }
        }

        public void Play(Song song)
        {
            if (song == null) throw new ArgumentNullException("song");

			_currentSong = song;

			if (_thread == null)
			{
				_thread = new System.Threading.Thread(new System.Threading.ThreadStart(this.PollingPlay));
				_thread.Start();
			}
        }

        public void Stop()
        {
            Engine.StopPlayback();
            Engine.Close();

            if (_thread.IsAlive)
                _thread.Abort();
            _thread = null;

            if (PlaybackStopped != null) PlaybackStopped(this, new EventArgs());
        }

        public void Next()
        {
            if (_thread == null) throw new InvalidOperationException("Unable to skip to next song while playback has stopped.");

            _currentSong = Player.QueueStatic.current;

            if (!Engine.OpenFile(_currentSong.FileName, TStreamFormat.sfAutodetect))
                throw new EngineException(ZPlayer.Engine.GetError());
            if (!Engine.StartPlayback())
                throw new EngineException(ZPlayer.Engine.GetError());

            if (SongChanged != null) SongChanged(this, new SongEventArgs(_currentSong));
        }

		private void PollingPlay()
		{
			if (!ZPlayer.Engine.OpenFile(_currentSong.FileName, libZPlay.TStreamFormat.sfAutodetect))
			{
				string error = ZPlayer.Engine.GetError();
				throw new EngineException(error);
			}

            if (!ZPlayer.Engine.StartPlayback())
            {
                throw new EngineException(ZPlayer.Engine.GetError());
            }

            if (PlaybackStarted != null) PlaybackStarted(this, new SongEventArgs(_currentSong));

			TStreamStatus status = default(TStreamStatus);
			TStreamInfo streamInfo = default(TStreamInfo);

			ZPlayer.Engine.GetStatus(ref status);
			ZPlayer.Engine.GetStreamInfo(ref streamInfo);
			TStreamTime totalTime = streamInfo.Length;

			Library.Song nextSong = _currentSong;

			while (status.fPlay)
			{
				ZPlayer.Engine.GetStreamInfo(ref streamInfo);
				totalTime = streamInfo.Length;

				// Enqueue for gapless playback
				if (status.nSongsInQueue == 0)
				{
					_currentSong = nextSong;
					nextSong = Player.QueueStatic.Dequeue();
					ZPlayer.Engine.AddFile(nextSong.FileName, TStreamFormat.sfAutodetect);
				}

				// Display running time
				TStreamTime time = default(TStreamTime);
				ZPlayer.Engine.GetPosition(ref time);

				Console2.Write(ConsoleColor.DarkGreen, "\r [Playing] {0} ", _currentSong.FileName);
				Console2.Write(ConsoleColor.Green, "{0:0}:{1:00} / {2:0}:{3:00}", time.hms.minute, time.hms.second, totalTime.hms.minute, totalTime.hms.second);

				System.Threading.Thread.Sleep(10);
				ZPlayer.Engine.GetStatus(ref status);
			}
		}

        private static ZPlay InitializeEngine()
        {
            ZPlay engine = new ZPlay();
            if (engine.SetSettings(TSettingID.sidAccurateLength, 1) == 0)
                throw new EngineException(engine.GetError());
            if (engine.SetSettings(TSettingID.sidAccurateSeek, 1) == 0)
                throw new EngineException(engine.GetError());
            return engine;
        }
    }
}
