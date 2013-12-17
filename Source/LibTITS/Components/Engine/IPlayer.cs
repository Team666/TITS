using System;
using TITS.Library;

namespace TITS.Components.Engine
{
    interface IPlayer
    {
        event EventHandler<SongEventArgs> PlaybackStarted;
        event EventHandler<SongEventArgs> SongChanged;
        event EventHandler PlaybackStopped;

        PlaybackStatus Status { get; }

        bool SupportsFileType(string extension);

        void Play(Song song);
        void Stop();
        void Next();
    }
}
