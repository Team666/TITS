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
}
