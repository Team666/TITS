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

namespace faceTITS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string SongTitle
        {
            get
            {
                if (App.Player.IsPlaying)
                    return App.Player.CurrentSong.ToString();
                return string.Empty;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            App.Player.Pause();

            if (App.Player.IsPlaying)
            {
                button.Content = "Pause";
            }
            else
            {
                button.Content = "Play";
            }
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            App.Player.Next();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            App.Player.Playlist = new TITS.Library.Playlist();
            App.Player.Playlist.AddFromDirectory(@"F:\Steam\steamapps\common\FTL Faster Than Light\FTL AE Soundtrack");
            App.Player.StartPlaying();
        }
    }
}
