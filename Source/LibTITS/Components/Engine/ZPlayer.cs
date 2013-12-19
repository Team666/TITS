using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TITS.Library;
using libZPlay;
using System.Diagnostics;

namespace TITS.Components.Engine
{
    class ZPlayer : IPlayer
    {
        private static string[] _supportedFileTypes = { ".mp3", ".mp2", ".mp1", ".ogg", ".flac", ".oga", ".aac", ".wav" };
        private Song _currentSong;
        private ZPlay _engine = null;
        private TCallbackFunc EngineCallback;

        public event EventHandler<SongEventArgs> PlaybackStarted;
        public event EventHandler<SongEventArgs> PlaybackPaused;
        public event EventHandler<SongEventArgs> SongChanged;
        public event EventHandler PlaybackStopped;

        public ZPlayer()
        {
            _engine = InitializeEngine();
        }

        public static string[] SupportedFileTypes
        {
            get { return _supportedFileTypes; }
        }

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

        public TimeSpan Position
        {
            get
            {
                TStreamTime time = default(TStreamTime);
                Engine.GetPosition(ref time);

                return time.ToTimeSpan();
            }
        }

        public bool SupportsFileType(string extension)
        {
            return SupportedFileTypes.Contains(extension);
        }

        internal ZPlay Engine
        {
            get
            {
                if (_engine == null)
                    _engine = InitializeEngine();
                return _engine;
            }
            private set
            {
                _engine = value;
            }
        }

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

        public void Stop()
        {
            Engine.StopPlayback();
            Engine.Close();

            if (PlaybackStopped != null) PlaybackStopped(this, new EventArgs());
        }

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

        public void Next()
        {
            lock (_engine)
            {
                // Get the next song from the queue
                _currentSong = Player.QueueStatic.current;

                if (!Engine.OpenFile(_currentSong.FileName, TStreamFormat.sfAutodetect))
                    throw new EngineException(Engine.GetError());
                if (!Engine.StartPlayback())
                    throw new EngineException(Engine.GetError());

                Queue();
            }

            if (SongChanged != null) SongChanged(this, new SongEventArgs(_currentSong));
        }

        private void Queue()
        {
            Song next = Player.QueueStatic.Dequeue();
            Queue(next);
        }

        private void Queue(Song song)
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
                        _currentSong, Player.QueueStatic.current);

                    _currentSong = Player.QueueStatic.current;
                    if (SongChanged != null) SongChanged(this, new SongEventArgs(_currentSong));
                    Queue();
                    break;
                default:
                    Debug.WriteLine("Unhandled engine callback: {0}", msg);
                    break;
            }

            return 0;
        }

        private ZPlay InitializeEngine()
        {
            ZPlay engine = new ZPlay();
            if (engine.SetSettings(TSettingID.sidAccurateLength, 1) == 0)
                throw new EngineException(engine.GetError());
            if (engine.SetSettings(TSettingID.sidAccurateSeek, 1) == 0)
                throw new EngineException(engine.GetError());

            EngineCallback = new TCallbackFunc(Callback);
            if (!engine.SetCallbackFunc(EngineCallback, TCallbackMessage.MsgNextSongAsync, 0))
                throw new EngineException(engine.GetError());

            return engine;
        }
    }
}
