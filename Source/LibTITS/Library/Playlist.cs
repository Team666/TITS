using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TITS.Components;

namespace TITS.Library
{
    public class Playlist : List<Song>
    {
        private Playlist _original;
        private int _index = -1;

        public static readonly Playlist Empty = new Playlist();
        public RepeatModes RepeatMode { get; set; }

        public Playlist() : base()
        {
        }

        /// <summary>
        /// Gets whether the playlist is empty or not.
        /// </summary>
        public bool IsEmpty
        {
            get { return (base.Count == 0); }
        }

        /// <summary>
        /// Gets the currently playing song in the playlist.
        /// </summary>
        public Song CurrentSong
        {
            get
            {
                if (_index < 0)
                    return null;

                return this[Index];
            }
        }

        /// <summary>
        /// Gets the next song in the playlist.
        /// </summary>
        /// <param name="forcedNext">indicates if playlist should give the next song regardless of RepeatMode: Single
        /// this is useful for when the user explicitly requested a next song</param>
        public Song NextSong(bool peek = true, bool forcedNext = false)
        {
            if (RepeatMode == RepeatModes.Track && !forcedNext)
            {
                return this[_index];   
            }

            if (base.Count == 0)
            {
                return null;
            }

            int nextSongIndex = CalculateIndex(_index + 1);

            // Next song is current song, playlist is done.
            // Note that playback can stop here only if no repeat mode is on
            if (nextSongIndex == _index && RepeatMode == RepeatModes.None)
            {
                return null;
            }

            if(!peek)
            {
                _index = nextSongIndex;
            }

            return this[nextSongIndex];
        }

        /// <summary>
        /// Sets index to the song before current song and returns the new indexed song.
        /// </summary>
        public Song PreviousSong(bool peek = true)
        {
            if (base.Count == 0)
            {
                return null;
            }
   
            // Nothing has been played yet or only the first song is playing
            // Note: This might change, right now it is just implemented as the "normal" case below
            if (_index < 1)
            {
                // If repeat: false, will return 0 but that's fine too...
                var newIndex = CalculateIndex(_index - 1);

                if (!peek)
                {
                    _index = newIndex;
                }

                return this[newIndex];
            }
            else
            {
                var newIndex = CalculateIndex(_index - 1);

                if (!peek)
                {
                    _index = newIndex;
                }

                return this[_index];
            }
        }

        /// <summary>
        /// Gets or sets the index of the current song.
        /// </summary>
        public int Index
        {
            get
            { 
                return _index; 
            }
        }

        public void SetIndex(int newIndex)
        {
            _index = CalculateIndex(newIndex);
        }

        /// <summary>
        /// Useful to calculate the index from given value.
        /// Will either wraparound or give last possible index.
        /// </summary>
        public int CalculateIndex(int value)
        {
            int count = base.Count;
            bool repeatAll = RepeatMode == RepeatModes.All;

            if (repeatAll)
            {
                if (value < 0)
                {   
                    while (value < 0)
                    {
                        value += count;
                    }

                    return value;
                }
                else if (value >= count)
                {
                    return value % count;
                }
                else
                {
                    return value;
                }
            }
            else
            {
                if (value < 0)
                {
                    return 0;
                }
                else if (value >= count)
                {
                    return count - 1;
                }
                else
                {
                    return value;
                }                
            }
        }

        public void OffsetIndexBy(int offset)
        {
            _index = CalculateIndex(_index + offset);
        }

        /// <summary>
        /// Initializes a new playlist.
        /// </summary>
        /// <param name="path">The path to the directory or playlist file containing files to add.</param>
        /// <returns>A new playlist containing the specified items, or an empty playlist.</returns>
        public static Playlist Load(string path)
        {
            if (Directory.Exists(path))
                return LoadFromDirectory(path);
            else if (File.Exists(path))
                return LoadFromFile(path);
            else
                return Playlist.Empty;
        }

        /// <summary>
        /// Initializes a new playlist based on the specified directory.
        /// </summary>
        /// <param name="path">The directory containing the files to add.</param>
        /// <param name="recursive">True to recursively search directories for files to add.</param>
        /// <returns>A new playlist containing the files from the specified directory.</returns>
        public static Playlist LoadFromDirectory(string path, bool recursive = true)
        {
            Playlist jouwgezicht = new Playlist();
            jouwgezicht.AddFromDirectory(path, recursive);
            return jouwgezicht;
        }

        /// <summary>
        /// Initializes a new playlist based on the specified playlist file.
        /// </summary>
        /// <param name="path">The path to the playlist file to add.</param>
        /// <returns>A new playlist containing the files from the playlist file.</returns>
        public static Playlist LoadFromFile(string path)
        {
            Playlist jouwgezicht = new Playlist();
            jouwgezicht.AddFromFile(path);
            return jouwgezicht;
        }

        /// <summary>
        /// Adds the specified file or directory to the playlist.
        /// </summary>
        /// <param name="path">The path to the music file or directory containing music files.</param>
        public void Add(string path)
        {
            if (Directory.Exists(path))
                AddFromDirectory(path);
            else if (File.Exists(path))
                Add(new Song(path));
            else
                System.Diagnostics.Trace.WriteLine("Attempted to add non-existing file or directory to playlist " + path);
        }

        /// <summary>
        /// Adds the specified song to the end of the playlist.
        /// </summary>
        /// <param name="item">The song to be added.</param>
        public new void Add(Song item)
        {
            if (item != null)
            {
                var extension = Path.GetExtension(item.FileName).ToLower();
                var isSupported = Components.Engine.Player.SupportedFileTypes.Contains(extension);
                if (isSupported)
                {
                    base.Add(item);
                }
                else
                {
                    System.Diagnostics.Trace.WriteLine(item.FileName + " is not supported; not added to playlist.");
                }
            }
        }

        /// <summary>
        /// Adds music files from the specified directory to the playlist.
        /// </summary>
        /// <param name="path">The directory containing the files to add.</param>
        /// <param name="recursive">True to recursively search directories for files to add.</param>
        public void AddFromDirectory(string path, bool recursive = true)
        {
            foreach (string file in Directory.EnumerateFiles(path, "*.*", (recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)))
            {
                FileInfo fi = new FileInfo(file);
                if (!fi.Attributes.HasFlag(FileAttributes.Hidden))
                    Add(new Song(file));
            }
        }

        /// <summary>
        /// Adds music files from the specified playlist file.
        /// </summary>
        /// <param name="path">The path to the playlist file.</param>
        public void AddFromFile(string path)
        {
            byte[] head = Utility.PeekFile(path, 5);
            switch (Encoding.UTF8.GetString(head))
            {
                case "<?zpl": // Zune Playlist
                    System.Xml.XmlDocument xd = new System.Xml.XmlDocument();
                    xd.Load(path);
                    foreach (System.Xml.XmlNode media in xd.SelectNodes("/smil/body/seq/media"))
                    {
                        if (media.Attributes["src"] != null)
                        {
                            string src = media.Attributes["src"].Value;
                            Add(new Song(src));
                        }
                    }
                    break;

                default:
                    System.Diagnostics.Trace.WriteLine("Unsupported playlist format: " + path, "Debug");
                    break;
            }
        }

        public void Shuffle()
        {
            _original = this;

            var random = new Random();
            for (int n = Count - 1; n > 1; n--)
            {
                int k = random.Next(n);

                Song value = this[k];
                this[k] = this[n];
                this[n] = value;
            }
        }
    }
}
