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

        public static string[] SupportedFileTypes
        {
            get { return _supportedFileTypes; }
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
			_currentSong = song;
			if (_thread == null)
			{
				_thread = new System.Threading.Thread(new System.Threading.ThreadStart(this.PollingPlay));
				_thread.Start();
			}
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

			TStreamStatus status = default(TStreamStatus);
			TStreamInfo streamInfo = default(TStreamInfo);

			ZPlayer.Engine.GetStatus(ref status);
			ZPlayer.Engine.GetStreamInfo(ref streamInfo);
			TStreamTime totalTime = streamInfo.Length;

			while (status.fPlay)
			{
				ZPlayer.Engine.GetStreamInfo(ref streamInfo);
				totalTime = streamInfo.Length;

				// Enqueue for gapless playback
				if (status.nSongsInQueue == 0)
				{
					ZPlayer.Engine.AddFile(Player.QueueStatic.Dequeue().FileName, TStreamFormat.sfAutodetect);
				}

				// Display running time
				TStreamTime time = default(TStreamTime);
				ZPlayer.Engine.GetPosition(ref time);

				Console2.Write(ConsoleColor.DarkGreen, "[Playing] ");
				Console2.Write(ConsoleColor.Green, "{0:0}:{1:00} / {2:0}:{3:00} \r", time.hms.minute, time.hms.second, totalTime.hms.minute, totalTime.hms.second);

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
