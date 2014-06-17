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

        // SICK HACK ~~ OMG ~~ ELITE NO SCOPE
        private void PlaylistListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var context = (faceTITS.ViewModels.NowPlayingPlaylistModel)this.DataContext;
            context.HookupHandlers();
        }
    }
}
