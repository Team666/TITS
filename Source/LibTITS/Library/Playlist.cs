using System;
using System.Collections.Generic;
using System.IO;
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

        public static Playlist LoadFromDirectory(string filepath)
        {
            Playlist jouwgezicht = new Playlist();

            foreach (string file in Directory.EnumerateFiles(filepath, "*.*", SearchOption.TopDirectoryOnly))
            {
                FileInfo fi = new FileInfo(file);
                bool isSupported = Components.Engine.Player.SupportedFileTypes.Contains(fi.Extension);
                if (!fi.Attributes.HasFlag(FileAttributes.Hidden) && isSupported)
                    jouwgezicht.Add(new Song(file));
            }

            return jouwgezicht;
        }
    }
}
