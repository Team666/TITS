using libZPlay;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
