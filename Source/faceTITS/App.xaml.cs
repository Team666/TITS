using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TITS;

namespace faceTITS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static TITS.Components.NowPlaying _player;
        private Keyboard.KeyboardListener _keyboardListener;

        public static TITS.Components.NowPlaying Player
        {
            get
            {
                if (_player == null)
                {
                    _player = new TITS.Components.NowPlaying();
                }
                return _player;
            }
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            _keyboardListener = new Keyboard.KeyboardListener();
            _keyboardListener.KeyDown += Global_KeyDown;
        }

        private void Global_KeyDown(object sender, Keyboard.KeyEventArgs args)
        {
            switch (args.Key)
            {
                case System.Windows.Input.Key.MediaPlayPause:
                    if (Player.Status == TITS.Components.Engine.PlaybackStatus.Stopped)
                    {
                        Player.StartPlaying();
                    }
                    else
                    {
                        Player.Pause();
                    }
                    break;

                case System.Windows.Input.Key.MediaPreviousTrack:
                    Player.Previous();
                    break;

                case System.Windows.Input.Key.MediaNextTrack:
                    Player.Next(forcedNext: true);
                    break;

                case System.Windows.Input.Key.MediaStop:
                    Player.Stop();
                    break;
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            if (_keyboardListener != null)
            {
                _keyboardListener.Dispose();
                _keyboardListener = null;
            }
        }
    }
}
