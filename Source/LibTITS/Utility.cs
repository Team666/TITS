using libZPlay;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TITS
{
    public static class Utility
    {
        /// <summary>
        /// Gets a <see cref="System.TimeSpan"/> that represents this instance.
        /// </summary>
        /// <param name="time">The <see cref="ZPlay.TStreamTime"/> to convert.</param>
        /// <returns>A new <see cref="System.TimeSpan"/> instance that represents <paramref name="time"/>.</returns>
        public static TimeSpan ToTimeSpan(this TStreamTime time)
        {
            return new TimeSpan(0, (int)time.hms.hour, (int)time.hms.minute, (int)time.hms.second, (int)time.hms.millisecond);
        }

        /// <summary>
        /// Returns the first few bytes from the beginning of a file.
        /// </summary>
        /// <param name="path">The path to the file to read.</param>
        /// <param name="length">The number of bytes to read.</param>
        /// <returns>A byte array that contains the at most <paramref name="length"/> bytes, or null if the file could not be read.</returns>
        public static byte[] PeekFile(string path, int length = 4)
        {
            byte[] buffer = new byte[length];

            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    if (fs.CanRead && fs.Read(buffer, 0, buffer.Length) > 0)
                    {
                        fs.Close();
                        return buffer;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex);
            }

            return null;
        }
    }
}
