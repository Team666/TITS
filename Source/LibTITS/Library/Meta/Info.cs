using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TITS.Library.Meta
{
    public class SongInfo
    {
        public int Track { get; set; }
        public string Title { get; set; }
        public AlbumInfo Album { get; set; }
        public ArtistInfo Artist { get; set; }

        public SongInfo(libZPlay.TID3InfoEx id3)
        {
            Title = id3.Title;
            Album = new AlbumInfo(id3.Album, id3.AlbumArtist);
            Artist = new ArtistInfo(id3.Artist);

            int track = 0;
            if (int.TryParse(id3.Track, out track))
                Track = track;
        }

        public override string ToString()
        {
            return string.Format("{0}. {1} - {2}", Track, Artist, Title);
        }
    }

    public class AlbumInfo
    {
        public string Name { get; set; }
        public ArtistInfo AlbumArtist { get; set; }

        public AlbumInfo(string name, string artistName)
        {
            Name = name;
            AlbumArtist = new ArtistInfo(artistName);
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class ArtistInfo
    {
        public string Name { get; set; }

        public ArtistInfo(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
