TITS
====

Overview
--------

TITS is the codename for a music player. For details, see http://horsedrowner.net/tits.

Quick Start
-----------

1. Clone the repository.
2. Wait.

Planned features
----------------

### Must have

* **Search by typing anywhere**

  Results are displayed in each view (albums in albums, songs in songs, artists in artists); a
  search is executed in those three categories at the same time. Pressing <kbd>Enter</kbd> should
  play the selected album/song/artist, and another key combination (e.g.
  <kbd>Alt</kbd>+<kbd>Q</kbd>) should enqueue the selected item.

  Typing anywhere should initiate this search. Alternatively, it should be possible to invoke this
  in a dialog for the current playlist, when the main window is not active. Compare with
  [Winamp](http://www.winamp.com/)'s "Jump to file" dialog.

* **Queue after current track**

  [Spotify](http://www.spotify.com/) does this very well. Dragging any item (be it a song, album,
  artist of playlist) to the "Now playing" playlist would enqueue it. Additionally,
  [Foobar2000](http://www.foobar2000.org/) has the options *Cursor follows playback* and *Playback
  follows cursor*.

  Queued songs should have a visual hint, such as "(2)", next to the title.

* **~Multi-dimensional~ rating system**
  
  Most music players have 2 to 5 levels of rating (Such as *starred/not starred*, or *3/5* stars).
  Foobar2000 doesn't have *any* rating system built-in. I would take Zune's *Like/Unrated/Dislike*
  system (and possibly tie that with Last.fm's *love/ban* system). You could introduce additional,
  separate ratings, for example "Funny". This could also double as a form of tagging.

  *Banned* items would never be played when playing music at random, but would still be playing when
  listening to the entire album in order. Similarly, *loved* items should be played more often.

  Ratings could also apply not just to songs, but albums and entire artists.

### Should have

* **[Last.fm](http://www.last.fm/) scrobbling**

  The [Last.fm API](http://www.last.fm/api) offers more functionality than just scrobbling. For
  example, a "Now playing" screen could display what friends are listening to, and any friends that
  might love the currently playing song.

* **Replay gain support for MP3 files**

* ***Intro*-tags and multi-part songs**

  Songs tagged as *intro* should not be played in shuffle mode. Similarly, songs with multiple parts
  should be treated as a single item, and should always be played after each order.

### Nice to have
* **Intelligent sort modes**
  
  For example, sorting by _most played_ albums where the most played albums 'deteriorate' faster than 
  lesser played albums (to prevent certain albums from dominating the list for a long period of time
  so it could ~actively reflect the changing listening habits of the user~).

* **Full Last.fm integration**

  Just like the Zune Software was relying on the Zune Marketplace for Artist information and
  pictures, TITS could mimick and extend upon the behaviour of the Zune Software by retrieving
  relevant information from Last.fm. This also includes a pretty user avatar displayed in the player
  itself.

* **Windows Phone 8 Sync Support**

  Windows Phone 8 now uses the 
  [Media Transfer Protocol](http://en.wikipedia.org/wiki/Media_Transfer_Protocol), 
  this functionality should be fairly easy to include in the player by using the API provided by 
  Microsoft.

* **Warn when a track is paused for longer than 10 minutes**

  I'm often listening to music when a friend sends me a link to, for example, a YouTube video, so I
  pause my music and watch it. However, I often forget about the paused track until later.

* **Tracking play/skip ratio**

  Some way of tracking how often you've played a song or skipped it would be useful for several
  reasons. It would, for instance, allow you to see which songs you actually listen to, and which
  songs you usually skip without realizing.

  One way to show this, would be to show a percentage for each song that indicates the average
  length played. 100% would indicate a song you never skipped, and 0% would indicate a song you
  never actually listened.

Milestones
----------
* **v0.001 Alpha - 'Will Fail'**

  *Playback of files in a single directory achieved*
