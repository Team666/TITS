using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace faceTITS
{
	/// <summary>
	/// Interaction logic for View1.xaml
	/// </summary>
	public partial class View1 : UserControl
	{
        private View1Model _viewModel;
		public View1()
		{
			this.InitializeComponent();
            this._viewModel = new View1Model();
			// Insert code required on object creation below this point.
		}

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            View1Model.Play();
        }

        private void NextsongButton_Click(object sender, RoutedEventArgs e)
        {
            View1Model.NextSong();
        }

        private void PreviousSongButton_Click(object sender, RoutedEventArgs e)
        {
            View1Model.PreviousSong();
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            App.Player.Playlist = new TITS.Library.Playlist();
            App.Player.Playlist.AddFromDirectory(@"C:\Users\Coolicer\Music\Daft Punk\Tron Legacy Original Motion Picture Soundtrack");
        }
	}
}