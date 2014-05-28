using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TITS.Components.Engine
{
    /// <summary>
    /// Represents the waiting queue for the next song to be played.
    /// It is only used for the gapless playback of the next logical song.
    /// </summary>
	class EngineQueue : Queue<Library.Song>
	{
        // Must only be called by this.Dequeue to notify the taking out of next song
        private Action NotifyPlaylistOfDequeueAction;

        /// <summary>
        /// Occurs when the queue is emptied.
        /// </summary>
		public event EventHandler QueueEmpty;

        /// <summary>
        /// Initializes a new instance of the engine queue.
        /// </summary>
        public EngineQueue(Action NotifyPlaylistOfDequeueAction) : base(1)
        {
            this.NotifyPlaylistOfDequeueAction = NotifyPlaylistOfDequeueAction;
        }

        /// <summary>
        /// Pulls the next song from the queue.
        /// </summary>
        /// <returns>The dequeued song.</returns>
		public new Library.Song Dequeue()
		{
			Library.Song song = base.Dequeue();

            // Engine has just taken the next song so we must notify the playlist of this
            NotifyPlaylistOfDequeueAction();

            if (this.Count == 0 && QueueEmpty != null)
            {
                QueueEmpty(this, new EventArgs());
            }

			return song;
		}

        public void Flush()
        {
            // Shouldn't contain more than one item
            // but nonetheless it is prudent to not assume this
            while (this.Count > 0)
            {
                base.Dequeue();
            }

            if (QueueEmpty != null)
            {
                QueueEmpty(this, new EventArgs());
            }
        }
	}
}
