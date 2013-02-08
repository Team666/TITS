using System;
using TITS.Library;

namespace TITS.Components.Engine
{
    interface IPlayer
    {
        bool SupportsFileType(string extension);
        string[] GetSupportedFileTypes();

        void Play(Song song);
    }
}
