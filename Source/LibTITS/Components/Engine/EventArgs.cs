using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TITS.Components.Engine
{
    public class SongEventArgs : EventArgs 
    {
        public SongEventArgs(Library.Song song)
        {
            this.Song = song;
        }

        public Library.Song Song { get; private set; }
    }

    public class VolumeEventArgs : EventArgs
    {
        public VolumeEventArgs(int volume)
        {
            this.Volume = volume;
        }

        public int Volume { get; private set; }
    }
}
