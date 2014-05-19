using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using libZPlay;
using System.Diagnostics;

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
        private Meta.SongInfo _meta;
        private bool _metaFailed;

        public string FileName { get; protected set; }

        public bool NowPlaying { get; protected set; }

        public Album Album { get; set; }

        public Meta.SongInfo Metadata 
        {
            get
            {
                if (_meta == null)
                    _meta = LoadMetadata();
                return _meta;
            }
            protected set { _meta = value; }
        }

        public TimeSpan Length { get; protected set; }

        public Song(string fileName)
        {
            if (fileName == null) throw new ArgumentNullException("fileName");
            if (!File.Exists(fileName)) throw new FileNotFoundException("File not found: " + fileName, fileName);

            this.FileName = fileName;
        }

        public override string ToString()
        {
            if (Metadata != null && !Metadata.IsEmpty)
                return Metadata.ToString();
            return FileName;
        }

        protected Meta.SongInfo LoadMetadata()
        {
            if (_metaFailed) return null;

            ZPlay engine = new ZPlay();
            TID3InfoEx info = default(TID3InfoEx);

            if (!engine.LoadFileID3Ex(FileName, TStreamFormat.sfAutodetect, ref info,
                fDecodePicture: true))
            {
                _metaFailed = true;
                Trace.WriteLine("Failed to load metadata of file " + FileName, "Warning");
                return null;
            }

            return new Meta.SongInfo(info);
        }
    }
}
