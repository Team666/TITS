using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TITS.Library;
using libZPlay;
using System.Diagnostics;

namespace TITS.Components.Engine
{
    /// <summary>
    /// Represents the libZPlay engine.
    /// </summary>
    class ZPlayer : IPlayer
    {
        private static string[] _supportedFileTypes = { ".mp3", ".mp2", ".mp1", ".ogg", ".flac", ".oga", ".aac", ".wav" };
        private Song _currentSong;
        private ZPlay _engine = null;
        private TCallbackFunc EngineCallback;

        /// <summary>
        /// Occurs when the engine has started or resumed playback.
        /// </summary>
        public event EventHandler<SongEventArgs> PlaybackStarted;

        /// <summary>
        /// Occurs when the engine has paused playback.
        /// </summary>
        public event EventHandler<SongEventArgs> PlaybackPaused;

        /// <summary>
        /// Occurs when the engine has started playing a different song.
        /// </summary>
        public event EventHandler<SongEventArgs> SongChanged;

        /// <summary>
        /// Occurs when the engine has stopped playback.
        /// </summary>
        public event EventHandler PlaybackStopped;

        /// <summary>
        /// Initializes a new instance of libZPlay.
        /// </summary>
        public ZPlayer()
        {
            _engine = new ZPlay();
            if (_engine.SetSettings(TSettingID.sidAccurateLength, 1) == 0)
                throw new EngineException(_engine.GetError());
            if (_engine.SetSettings(TSettingID.sidAccurateSeek, 1) == 0)
                throw new EngineException(_engine.GetError());

            EngineCallback = new TCallbackFunc(Callback);
            if (!_engine.SetCallbackFunc(EngineCallback, TCallbackMessage.MsgNextSongAsync, 0))
                throw new EngineException(_engine.GetError());
        }

        /// <summary>
        /// Gets a list of file extensions that this engine supports.
        /// </summary>
        public static string[] SupportedFileTypes
        {
            get { return _supportedFileTypes; }
        }

        /// <summary>
        /// Gets the current playback status.
        /// </summary>
        public PlaybackStatus Status
        {
            get
            {
                TStreamStatus status = default(TStreamStatus);
                Engine.GetStatus(ref status);

                if (status.fPause)
                    return PlaybackStatus.Paused;
                else if (status.fPlay)
                    return PlaybackStatus.Playing;
                else
                    return PlaybackStatus.Stopped;
            }
        }

        /// <summary>
        /// Gets the current playback position.
        /// </summary>
        public TimeSpan Position
        {
            get
            {
                TStreamTime time = default(TStreamTime);
                Engine.GetPosition(ref time);

                return time.ToTimeSpan();
            }
        }

        /// <summary>
        /// Determines whether an engine exists that supports the specified extension.
        /// </summary>
        /// <param name="extension">The file extension including leading period.</param>
        /// <returns>True if files with the specified extension can be played.</returns>
        public bool SupportsFileType(string extension)
        {
            return SupportedFileTypes.Contains(extension);
        }

        /// <summary>
        /// Gets the internal ZPlay instance.
        /// </summary>
        internal ZPlay Engine
        {
            get { return _engine; }
        }

        /// <summary>
        /// Starts playback with the specified song.
        /// </summary>
        /// <param name="song">The song to be played.</param>
        public void Play(Song song)
        {
            if (song == null) throw new ArgumentNullException("song");

            _currentSong = song;

            Debug.WriteLine("Playing {0}", song);

            if (!Engine.OpenFile(_currentSong.FileName, TStreamFormat.sfAutodetect))
                throw new EngineException(Engine.GetError());
            if (!Engine.StartPlayback())
                throw new EngineException(Engine.GetError()); 
            
            if (PlaybackStarted != null) PlaybackStarted(this, new SongEventArgs(_currentSong));

            Queue();
        }

        /// <summary>
        /// Stops playback.
        /// </summary>
        public void Stop()
        {
            Engine.StopPlayback();
            Engine.Close();

            if (PlaybackStopped != null) PlaybackStopped(this, new EventArgs());
        }

        /// <summary>
        /// Pauses or resumes playback.
        /// </summary>
        public void Pause()
        {
            if (Status == PlaybackStatus.Playing)
            {
                Engine.PausePlayback();
                if (PlaybackPaused != null) PlaybackPaused(this, new SongEventArgs(_currentSong));
            }
            else if (Status == PlaybackStatus.Paused)
            {
                Engine.ResumePlayback();
                if (PlaybackStarted != null) PlaybackStarted(this, new SongEventArgs(_currentSong));
            }
        }

        /// <summary>
        /// Skips the current song and plays the next song from the queue.
        /// </summary>
        public void Next()
        {
            lock (_engine)
            {
                // Get the next song from the queue
                _currentSong = Player.QueueStatic.Current;

                if (!Engine.OpenFile(_currentSong.FileName, TStreamFormat.sfAutodetect))
                    throw new EngineException(Engine.GetError());
                if (!Engine.StartPlayback())
                    throw new EngineException(Engine.GetError());

                Queue();
            }

            if (SongChanged != null) SongChanged(this, new SongEventArgs(_currentSong));
        }

        /// <summary>
        /// Queues the next song for playback.
        /// </summary>
        public void Queue()
        {
            Song next = Player.QueueStatic.Dequeue();
            Queue(next);
        }

        /// <summary>
        /// Queues the specified song for playback.
        /// </summary>
        /// <param name="song">The song to queue.</param>
        public void Queue(Song song)
        {
            Debug.WriteLine("Queueing {0}", song);
            Engine.AddFile(song.FileName, TStreamFormat.sfAutodetect);
        }

        private int Callback(uint objptr, int user_data, TCallbackMessage msg, 
            uint param1, uint param2)
        {
            switch (msg)
            {
                case TCallbackMessage.MsgNextSongAsync:
                    // param1: index of playing song 
                    // param2: number of songs remaining in gapless queue
                    // return: not used 
                    Debug.WriteLine("MsgNextSongAsync: {0} => {1}", 
                        _currentSong, Player.QueueStatic.Current);

                    _currentSong = Player.QueueStatic.Current;
                    if (SongChanged != null) SongChanged(this, new SongEventArgs(_currentSong));
                    Queue();
                    break;
                default:
                    Debug.WriteLine("Unhandled engine callback: {0}", msg);
                    break;
            }

            return 0;
        }
    }
}
