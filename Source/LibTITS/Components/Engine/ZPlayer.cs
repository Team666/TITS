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
        private Player _parent;
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
        /// Occurs when the volume has changed.
        /// </summary>
        public event EventHandler<VolumeEventArgs> VolumeChanged;

        /// <summary>
        /// Initializes a new instance of libZPlay.
        /// </summary>
        public ZPlayer(Player parent)
        {
            _parent = parent;
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
        /// Gets the current song's Length
        /// </summary>
        public TimeSpan Length
        {
            get
            {
                TStreamInfo streamInfo = new TStreamInfo();
                Engine.GetStreamInfo(ref streamInfo);

                return streamInfo.Length.ToTimeSpan();
            }
        }

        /// <summary>
        /// Gets or sets the player volume as a value from 0 to 100.
        /// </summary>
        public int Volume
        {
            get
            {
                int left = 0;
                int right = 0;

                Engine.GetPlayerVolume(ref left, ref right);

                return (left + right) / 2;
            }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                else if (value > 100)
                {
                    value = 100;
                }

                Engine.SetPlayerVolume(value, value);
                if (VolumeChanged != null)
                {
                    VolumeChanged(this, new VolumeEventArgs(value));
                }
            }
        }

        private Song _currentSong;
        public Song CurrentSong
        {
            get
            {
                return _currentSong;
            }

            private set
            {
                // Old song
                if (_currentSong != null)
                {
                    _currentSong.NowPlaying = false;
                }

                // Set new song
                _currentSong = value;
                _currentSong.NowPlaying = true;
            }
        }

        /// <summary>
        /// Gets the internal ZPlay instance.
        /// </summary>
        internal ZPlay Engine
        {
            get
            {
                return _engine;
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
        /// Starts playback with the specified song.
        /// </summary>
        /// <param name="song">The song to be played.</param>
        public void Play(Song song)
        {
            if (song == null)
            {
                throw new ArgumentNullException("song");
            }

            Debug.WriteLine("Playing {0}", song);

            ChangeSong(song);

            if (PlaybackStarted != null)
            {
                PlaybackStarted(this, new SongEventArgs(CurrentSong));
            }
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
                if (PlaybackPaused != null) PlaybackPaused(this, new SongEventArgs(CurrentSong));
            }
            else if (Status == PlaybackStatus.Paused)
            {
                Engine.ResumePlayback();
                if (PlaybackStarted != null) PlaybackStarted(this, new SongEventArgs(CurrentSong));
            }
        }

        /// <summary>
        /// Immediately changes the song, starts playback and queues the next song.
        /// </summary>
        /// <param name="song">The song to change to.</param>
        public void ChangeSong(Library.Song song)
        {
            lock (_engine)
            {
                CurrentSong = song;

                if (!Engine.OpenFile(song.FileName, TStreamFormat.sfAutodetect))
                {
                    throw new EngineException(Engine.GetError());
                }

                if (!Engine.StartPlayback())
                {
                    throw new EngineException(Engine.GetError());
                }

                Enqueue();
            }

            if (SongChanged != null)
            {
                SongChanged(this, new SongEventArgs(song));
            }
        }

        /// <summary>
        /// Queues the next song for playback.
        /// TODO: REFACTOR
        /// </summary>
        public void Enqueue()
        {
            if (_parent.Queue.Count > 0)
            {
                Song next = _parent.Queue.Peek();
                Queue(next);
            }
            else
            {
                Debug.WriteLine("Queue is empty");
            }
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

        private int Callback(uint objptr, int user_data, TCallbackMessage msg, uint param1, uint param2)
        {
            switch (msg)
            {
                case TCallbackMessage.MsgNextSongAsync:
                    // param1: index of playing song 
                    // param2: number of songs remaining in gapless queue
                    // return: not used 
                    var next = _parent.Queue.Dequeue();

                    Debug.WriteLine("MsgNextSongAsync: {0} => {1}", CurrentSong, next);

                    CurrentSong = next;
                    if (SongChanged != null)
                    {
                        SongChanged(this, new SongEventArgs(next));
                    }

                    Enqueue();
                    break;
                default:
                    Debug.WriteLine("Unhandled engine callback: {0}", msg);
                    break;
            }

            return 0;
        }
    }
}
