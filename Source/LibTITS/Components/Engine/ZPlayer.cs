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
            if (!ZPlayer.Engine.OpenFile(song.FileName, libZPlay.TStreamFormat.sfAutodetect))
                throw new EngineException(ZPlayer.Engine.GetError());

            _currentSong = song;
            TStreamInfo streamInfo = default(TStreamInfo);
            ZPlayer.Engine.GetStreamInfo(ref streamInfo);

            if (!ZPlayer.Engine.StartPlayback())
            {
                throw new EngineException(ZPlayer.Engine.GetError());
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
