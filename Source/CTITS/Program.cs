﻿using System;
using libZPlay;

namespace TITS
{
    class Program
    {
		[STAThread]
        static void Main(string[] args)
        {
            Console.Title = "TITS";

			TITS.Components.NowPlaying PleeTits = new Components.NowPlaying();

			//if (args == null || args.Length == 0)
			//{
			//	Console.WriteLine("CTITS [filename]\n");
			//	Console.WriteLine("\tfilename\tThe name of the file to play.");
			//	return;
			//}

            Console2.WriteLine(ConsoleColor.White, "TITS Console");
            try
            {
                //StartLoop(args[0]);
				string path = EnDanWat();

				PleeTits.Playlist = TITS.Library.Playlist.LoadFromDirectory(path);
				PleeTits.StartPlaying();
            }
            catch (Exception ex)
            {
                Console2.WriteLine(ConsoleColor.Yellow, ex.ToString());
            }
        }

		static string EnDanWat()
		{
			using (System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog())
			{
				dialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
				if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					string path = dialog.SelectedPath;
					return path;
				}
			}

			return null;
		}

        static void StartLoop(string filename)
        {
            ZPlay player = new ZPlay();

            Console.WriteLine("Opening {0}...", filename);
            player.SetSettings(TSettingID.sidAccurateLength, 1);
            player.SetSettings(TSettingID.sidAccurateSeek, 1);

            if (System.IO.File.Exists(filename.Replace("_loop", "_intro")))
                player.OpenFile(filename.Replace("_loop", "_intro"), TStreamFormat.sfAutodetect);
            else
                player.OpenFile(filename, TStreamFormat.sfAutodetect);

            if (!player.StartPlayback())
            {
                throw new Exception(player.GetError());
            }

            // ID3v2
            TID3Info info = default(TID3Info);
            if (player.LoadID3(TID3Version.id3Version2, ref info) && info.Title.Length > 0)
            {
                Console2.Write(ConsoleColor.Magenta, info.Title);
                Console2.Write(ConsoleColor.DarkMagenta, " - ");
                Console2.Write(ConsoleColor.Magenta, info.Artist);
                Console2.Write(ConsoleColor.DarkMagenta, " (");
                Console2.Write(ConsoleColor.Magenta, info.Album);
                Console2.WriteLine(ConsoleColor.DarkMagenta, ")");
            }

            // Get track length
            TStreamInfo streamInfo = default(TStreamInfo);
            player.GetStreamInfo(ref streamInfo);
            TStreamTime totalTime = streamInfo.Length;

            // Playback loop
            int playcount = 0;
            TStreamStatus status = default(TStreamStatus);
            player.GetStatus(ref status);
            while (status.fPlay)
            {
                player.GetStreamInfo(ref streamInfo);
                totalTime = streamInfo.Length;

                // Escape?
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Escape)
                    {
                        Console2.WriteLine(ConsoleColor.DarkRed, "[Stopped] ");
                        player.Close();
                        break;
                    }
                }

                // Enqueue for gapless playback
                if (status.nSongsInQueue == 0)
                {
                    playcount++;
                    player.AddFile(filename, TStreamFormat.sfAutodetect);
                }

                // Display running time
                TStreamTime time = default(TStreamTime);
                player.GetPosition(ref time);

                Console2.Write(ConsoleColor.DarkGreen, "[Playing] ");
                Console2.Write(ConsoleColor.Green, "{0:0}:{1:00} / {2:0}:{3:00} ", time.hms.minute, time.hms.second, totalTime.hms.minute, totalTime.hms.second);
                Console2.Write(ConsoleColor.DarkGreen, "({0})\r", playcount);

                System.Threading.Thread.Sleep(10);
                player.GetStatus(ref status);
            }
        }
    }
}
