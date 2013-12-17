using System;
using TITS.Library;

namespace TITS.Components.Engine
{
    interface IPlayer
    {
        event EventHandler<SongEventArgs> PlaybackStarted;
        event EventHandler<SongEventArgs> PlaybackPaused;
        event EventHandler<SongEventArgs> SongChanged;
        event EventHandler PlaybackStopped;

        PlaybackStatus Status { get; }
        TimeSpan Position { get; }

        bool SupportsFileType(string extension);

        void Play(Song song);
        void Stop();
        void Pause();
        void Next();
    }
}
