using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TITS.Library
{
    class Playlist : List<Song>
    {
        private Playlist _original;

        private int index = -1;

        public Playlist() : base()
        {
        }

        public Song CurrentSong
        {
            get
            {
                if (index == -1)
                    return null;

                return this[index];
            }
        }

        public Song NextSong
        {
            get
            {
                index++;
                return this[index];
            }
        }
    }
}
