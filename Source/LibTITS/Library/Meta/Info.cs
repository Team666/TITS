using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TITS.Library.Meta
{
    public struct SongInfo
    {
        string Title { get; set; }
        AlbumInfo Album { get; set; }
        ArtistInfo Artist { get; set; }
    }

    public struct AlbumInfo
    {
        string Name { get; set; }
        ArtistInfo AlbumArtist { get; set; }
    }

    public struct ArtistInfo
    {
        string Name { get; set; }
    }
}
