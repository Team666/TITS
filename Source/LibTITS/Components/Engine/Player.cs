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

        public EngineQueue Queue { get; private set; }

        public Player()
        {
            _zplayer = new ZPlayer();

            _supportedFileTypes = ZPlayer.SupportedFileTypes;
			Queue = new EngineQueue();
        }

        public static string[] SupportedFileTypes
        {
            get { return ZPlayer.SupportedFileTypes; }
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

    }
}
