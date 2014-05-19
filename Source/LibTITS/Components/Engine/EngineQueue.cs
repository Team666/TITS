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
        /// Occurs when the queue is emptied.
        /// </summary>
		public event EventHandler QueueEmpty;

        /// <summary>
        /// Initializes a new instance of the engine queue.
        /// </summary>
        public EngineQueue()
        {
            History = new Stack<Library.Song>();
        }

        /// <summary>
        /// Gets a history of songs that have been dequeued.
        /// </summary>
        public Stack<Library.Song> History { get; private set; }

        /// <summary>
        /// Pulls the next song from the queue.
        /// </summary>
        /// <returns>The dequeued song.</returns>
		public new Library.Song Dequeue()
		{
			Library.Song song = base.Dequeue();
            History.Push(song);

            if (this.Count == 0 && QueueEmpty != null)
                QueueEmpty(this, new EventArgs());

			return song;
		}
	}
}
