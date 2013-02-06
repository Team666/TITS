using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TITS.Library;
using libZPlay;

namespace TITS
{
    class Player
    {
        private Song _currentSong;
        private static ZPlay _engine = null;

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
            if (!Player.Engine.OpenFile(song.FileName, libZPlay.TStreamFormat.sfAutodetect))
                throw new EngineException(Player.Engine.GetError());

            _currentSong = song;
            TStreamInfo streamInfo = default(TStreamInfo);
            Player.Engine.GetStreamInfo(ref streamInfo);

            if (!Player.Engine.StartPlayback())
            {
                throw new EngineException(Player.Engine.GetError());
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
