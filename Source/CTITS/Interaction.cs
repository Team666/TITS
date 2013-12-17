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
    }
}
