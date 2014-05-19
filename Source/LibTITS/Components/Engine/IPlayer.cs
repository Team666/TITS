using System;
using TITS.Library;

namespace TITS.Components.Engine
{
    /// <summary>
    /// Represents a playback engine.
    /// </summary>
    interface IPlayer
    {
        /// <summary>
        /// Occurs when the engine has started or resumed playback.
        /// </summary>
        event EventHandler<SongEventArgs> PlaybackStarted;

        /// <summary>
        /// Occurs when the engine has paused playback.
        /// </summary>
        event EventHandler<SongEventArgs> PlaybackPaused;

        /// <summary>
        /// Occurs when the engine has started playing a different song.
        /// </summary>
        event EventHandler<SongEventArgs> SongChanged;

        /// <summary>
        /// Occurs when the engine has stopped playback.
        /// </summary>
        event EventHandler PlaybackStopped;
        
        /// <summary>
        /// Occurs when the volume has changed.
        /// </summary>
        event EventHandler<VolumeEventArgs> VolumeChanged;

        /// <summary>
        /// Gets the current playback status.
        /// </summary>
        PlaybackStatus Status { get; }

        /// <summary>
        /// Gets the current playback position.
        /// </summary>
        TimeSpan Position { get; }

        /// <summary>
        /// Gets or sets the player volume as a value from 0 to 100.
        /// </summary>
        int Volume { get; set; }

        Library.Song CurrentSong { get; }

        /// <summary>
        /// Determines whether an engine exists that supports the specified extension.
        /// </summary>
        /// <param name="extension">The file extension including leading period.</param>
        /// <returns>True if files with the specified extension can be played.</returns>
        bool SupportsFileType(string extension);

        /// <summary>
        /// Starts playback with the specified song.
        /// </summary>
        /// <param name="song">The song to be played.</param>
        void Play(Song song);

        /// <summary>
        /// Stops playback.
        /// </summary>
        void Stop();

        /// <summary>
        /// Pauses or resumes playback.
        /// </summary>
        void Pause();

        /// <summary>
        /// Skips the current song and plays the next song.
        /// </summary>
        void Next();

		void Previous();
    }
}
