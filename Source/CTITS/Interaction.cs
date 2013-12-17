using System;
using System.Collections.Generic;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace TITS
{
    static class Interaction
    {
        public static IEnumerable<string> BrowseFiles(bool isFolderPicker = false)
        {
            using (CommonOpenFileDialog dialog = new CommonOpenFileDialog())
            {
                dialog.IsFolderPicker = isFolderPicker;
                dialog.Multiselect = true;
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    return dialog.FileNames;
                }
            }

            return new string[0];
        }

        public static void PrintSong(Library.Song song, TimeSpan position, Components.Engine.PlaybackStatus status)
        {
            switch (status)
            {
                case TITS.Components.Engine.PlaybackStatus.Stopped:
                    Console2.Write(ConsoleColor.DarkGreen, "= ");
                    break;
                case TITS.Components.Engine.PlaybackStatus.Playing:
                    Console2.Write(ConsoleColor.DarkGreen, "> ");
                    break;
            }

            Console2.Write(ConsoleColor.DarkGreen, "{0} ", song);
            Console2.Write(ConsoleColor.Green, "{0:m\\:ss}\r", position);
        }
    }
}
