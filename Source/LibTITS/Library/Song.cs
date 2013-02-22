using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using libZPlay;

namespace TITS.Library
{
    public class Album
    {
        public Album()
        {
            Songs = new List<Song>();
        }

        public Image AlbumArt;
        public List<Song> Songs { get; private set; }
    }

    public class Song
    {
        public string FileName { get; protected set; }

        public bool NowPlaying { get; protected set; }

        public struct SongInfo
        {
            string Title  { get; set; }
            string Album  { get; set; }
            string Artist { get; set; }
        }

        public Album Album { get; set; }

        public SongInfo Info { get; protected set; }

        public TimeSpan Length { get; protected set; }

        public Song(string fileName)
        {
            if (fileName == null) throw new ArgumentNullException("fileName", "Parameter fileName must not be null");
            if (!File.Exists(fileName)) throw new FileNotFoundException("File not found: " + fileName, fileName);

            this.FileName = fileName;
        }
    }
}
