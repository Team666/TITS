using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TITS.Components.Engine
{
	class EngineQueue : Queue<Library.Song>
	{
		public event EventHandler QueueEmpty;

		public Library.Song Current { get { return _current; } }
		private Library.Song _current;

		public new Library.Song Dequeue()
		{
            if (this.Count == 0 && QueueEmpty != null)
                QueueEmpty(this, new EventArgs());

			Library.Song song = base.Dequeue();
			_current = song;

			return song;
		}
	}
}
