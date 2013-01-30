using System;
using System.Collections.Generic;
using System.IO;
using libZPlay;

namespace TITS.Library
{
    public class Song
    {
        public string FileName { get; protected set; }

        public bool IsLoaded { get; protected set; }

        public TimeSpan Length { get; protected set; }

        public Song(string fileName)
        {
            if (fileName == null) throw new ArgumentNullException("fileName", "Parameter fileName must not be null");
            if (!File.Exists(fileName)) throw new FileNotFoundException("File not found: " + fileName, fileName);

            this.FileName = fileName;
        }

        public void Load()
        {
            if (!Player.Engine.OpenFile(this.FileName, libZPlay.TStreamFormat.sfAutodetect))
            {
                throw new EngineException(Player.Engine.GetError());
            }
            else
            {
                this.IsLoaded = true;

                TStreamInfo streamInfo = default(TStreamInfo);
                Player.Engine.GetStreamInfo(ref streamInfo);

                this.Length = streamInfo.Length.ToTimeSpan();
            }
        }
    }
}
