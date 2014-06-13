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
using System.ComponentModel;

namespace faceTITS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            App.Player.Playlist = new TITS.Library.Playlist();
            App.Player.Playlist.AddFromDirectory(@"P:\Music\Greatest Hits");
            //App.Player.StartPlaying();

            this.MinWidth  = this.Width;
            this.MinHeight = this.Height;
        }
    }
}
