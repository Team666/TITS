using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TITS.Library
{
    public class Playlist : List<Song>
    {
        private Playlist _original;
        private int index = -1;

        public static readonly Playlist Empty = new Playlist();

        public Playlist()
            : base()
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
                if (index < 0)
                    return null;

                return this[Index];
            }
        }

        /// <summary>
        /// Gets the next song in the playlist.
        /// </summary>
        public Song NextSong
        {
            get
            {
                if (base.Count == 0)
                {
                    return null;
                }

                return this[(Index + 1) % base.Count];
            }
        }

        /// <summary>
        /// Gets or sets the index of the current song.
        /// </summary>
        public int Index
        {
            get
            { 
                return index; 
            }
            set
            {
                index = value;
            }
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
        /// Adds music files from the specified directory to the playlist.
        /// </summary>
        /// <param name="path">The directory containing the files to add.</param>
        /// <param name="recursive">True to recursively search directories for files to add.</param>
        public void AddFromDirectory(string path, bool recursive = true)
        {
            foreach (string file in Directory.EnumerateFiles(path, "*.*", (recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)))
            {
                FileInfo fi = new FileInfo(file);
                bool isSupported = Components.Engine.Player.SupportedFileTypes.Contains(fi.Extension);
                if (!fi.Attributes.HasFlag(FileAttributes.Hidden) && isSupported)
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
    }
}
