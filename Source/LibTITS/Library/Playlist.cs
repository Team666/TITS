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
                IncrementIndex();

                if (index >= 0)
                    return this[index];
                else
                    return null;
            }
        }

        private void IncrementIndex()
        {
            if (base.Count > 0)
                index = ++index % base.Count;
        }

        public static Playlist Load(string path)
        {
            if (Directory.Exists(path))
                return LoadFromDirectory(path);
            else if (File.Exists(path))
                return LoadFromFile(path);
            else
                return Playlist.Empty;
        }

        public static Playlist LoadFromDirectory(string filepath, bool recursive = false)
        {
            Playlist jouwgezicht = new Playlist();

            foreach (string file in Directory.EnumerateFiles(filepath, "*.*", (recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)))
            {
                FileInfo fi = new FileInfo(file);
                bool isSupported = Components.Engine.Player.SupportedFileTypes.Contains(fi.Extension);
                if (!fi.Attributes.HasFlag(FileAttributes.Hidden) && isSupported)
                    jouwgezicht.Add(new Song(file));
            }

            return jouwgezicht;
        }

        public static Playlist LoadFromFile(string path)
        {
            Playlist jouwgezicht = new Playlist();

            byte[] head = Utility.PeekFile(path, 5);
            switch (Encoding.UTF8.GetString(head))
            {
                case "<?zpl": // Zune Playlist: \smil\body\media[src]
                    // TODO
                    break;
                    
                default:
                    break;
            }

            return jouwgezicht;
        }
    }
}
