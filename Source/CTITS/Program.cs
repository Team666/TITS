using System;
using libZPlay;

namespace TITS
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "TITS";

            if (args == null || args.Length == 0)
            {
                Console.WriteLine("CTITS [filename]\n");
                Console.WriteLine("\tfilename\tThe name of the file to play.");
                return;
            }

            Console2.WriteLine(ConsoleColor.White, "TITS Console");
            try
            {
                StartLoop(args[0]);
            }
            catch (Exception ex)
            {
                Console2.WriteLine(ConsoleColor.Yellow, ex.ToString());
            }
        }

        static void StartLoop(string filename)
        {
            ZPlay player = new ZPlay();

            Console.WriteLine("Opening {0}...", filename);
            player.OpenFile(filename, TStreamFormat.sfAutodetect);
            player.StartPlayback();

            TID3Info info = default(TID3Info);
            if (player.LoadID3(TID3Version.id3Version2, ref info) && info.Title.Length > 0)
            {
                Console2.Write(ConsoleColor.Gray, "Playing ");
                Console2.Write(ConsoleColor.Magenta, info.Title);
                Console2.Write(ConsoleColor.Gray, " - ");
                Console2.WriteLine(ConsoleColor.Magenta, info.Artist);
            }

            TStreamStatus status = default(TStreamStatus);
            player.GetStatus(ref status);
            while (status.fPlay)
            {
                if (status.nSongsInQueue == 0)
                {
                    Console2.WriteLine(ConsoleColor.White, "Enqueueing");
                    player.AddFile(filename, TStreamFormat.sfAutodetect);
                }

                System.Threading.Thread.Sleep(100);
                player.GetStatus(ref status);
            }
        }
    }
}
