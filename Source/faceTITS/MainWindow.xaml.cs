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
using Microsoft.WindowsAPICodePack.Dialogs;

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

        private string SelectDirectory()
        {
            var dialog = new CommonOpenFileDialog();
            dialog.Title = "Select music directory to play";
            dialog.IsFolderPicker = true;
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);

            dialog.AddToMostRecentlyUsedList = false;
            dialog.AllowNonFileSystemItems = false;
            dialog.EnsureFileExists = true;
            dialog.EnsurePathExists = true;
            dialog.EnsureReadOnly = false;
            dialog.EnsureValidNames = true;
            dialog.Multiselect = false;
            dialog.ShowPlacesList = true;

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                return dialog.FileName;
            }
            else
            {
                return null;
            }
        }

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            App.Player.Playlist = new TITS.Library.Playlist();

            var path = SelectDirectory();
            App.Player.Playlist.AddFromDirectory(path);


            //App.Player.StartPlaying();

            this.MinWidth  = this.Width;
            this.MinHeight = this.Height;
        }
    }
}
