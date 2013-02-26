using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TITS.Components.Engine
{
	class EngineQueue : Queue<Library.Song>
	{
		public event EventHandler QueueEmpty;

		public new Library.Song Dequeue()
		{
			Library.Song song = base.Dequeue();

			if (this.Count == 0)
				QueueEmpty(this, new EventArgs());

			return song;
		}
	}
}
