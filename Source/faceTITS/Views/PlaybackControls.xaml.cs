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
	public partial class PlaybackControls : UserControl
	{
        public PlaybackControls()
		{
			this.InitializeComponent();

			// Insert code required on object creation below this point.
		}

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            var context = (PlaybackModel) this.DataContext;
            context.Play();
        }

        private void NextSongButton_Click(object sender, RoutedEventArgs e)
        {
            var context = (PlaybackModel)this.DataContext;
            context.NextSong();
        }

        private void PreviousSongButton_Click(object sender, RoutedEventArgs e)
        {
            var context = (PlaybackModel)this.DataContext;
            context.PreviousSong();
        }

        private void RepeatModeCycleButton_Click(object sender, RoutedEventArgs e)
        {
            var context = (PlaybackModel)this.DataContext;
            context.CycleRepeatMode();
        }
	}
}