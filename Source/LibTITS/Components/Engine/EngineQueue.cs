using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TITS.Components.Engine
{
    /// <summary>
    /// Represents a queue of upcoming songs, and holds a history of songs 
    /// that have been played.
    /// </summary>
	class EngineQueue : Queue<Library.Song>
	{
        /// <summary>
        /// Occurs when the queue is empty and there is nothing to dequeue.
        /// </summary>
		public event EventHandler QueueEmpty;

        /// <summary>
        /// Gets the song that was most recently removed from the queue.
        /// </summary>
        public Library.Song Current { get; private set; }

        /// <summary>
        /// Pulls the next song from the queue.
        /// </summary>
        /// <returns>The dequeued song.</returns>
		public new Library.Song Dequeue()
		{
            if (this.Count == 0 && QueueEmpty != null)
                QueueEmpty(this, new EventArgs());

			Library.Song song = base.Dequeue();
			Current = song;

			return song;
		}
	}
}
