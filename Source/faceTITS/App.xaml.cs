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

    }
}
