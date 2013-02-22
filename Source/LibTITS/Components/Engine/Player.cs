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

        public event EventHandler QueueEmpty;

        public Queue<Library.Song> Queue { get; private set; }

        public Player()
        {
            _zplayer = new ZPlayer();

            _supportedFileTypes = _zplayer.GetSupportedFileTypes();
        }

        public string[] GetSupportedFileTypes()
        {
            return _supportedFileTypes;
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
