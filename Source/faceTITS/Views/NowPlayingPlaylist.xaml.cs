using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace faceTITS.Views
{
    /// <summary>
    /// Interaction logic for NowPlayingPlaylist.xaml
    /// </summary>
    public partial class NowPlayingPlaylist : UserControl
    {
        public NowPlayingPlaylist()
        {
            InitializeComponent();
        }

        int _currentSongIndex;
        public int CurrentSongIndex
        {
            get
            {
                return _currentSongIndex;
            }

            set
            {
                _currentSongIndex = value;
                PlaylistListBox.ActiveItem = value;
                App.Player.Playlist.SetIndex(value, noCalculate: true);
            }
        }

        private void PlaylistListBox_Loaded(object sender, RoutedEventArgs e)
        {
            PlaylistListBox.SetStaticRefToSelf();
        }

    }
}
