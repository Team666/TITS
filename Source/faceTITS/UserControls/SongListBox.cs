using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Threading.Tasks;
using System.ComponentModel;

namespace faceTITS.UserControls
{
    class SongListBox : System.Windows.Controls.ListView
    {
        public SongListBox()
        {
        }

         //public static readonly DependencyProperty ActiveItemProperty = 
         //   DependencyProperty.RegisterAttached("ActiveItem", typeof(int), typeof(SongListBox), new PropertyMetadata(-1, onActiveItemChanged));

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is SongItem;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new SongItem();
        }
    }

    class SongItem : System.Windows.Controls.ListViewItem, INotifyPropertyChanged
    {
        public SongItem() : base()
        {
            
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            if (newContent == BindingOperations.DisconnectedSource)
                return;

            if (oldContent != null)
            {
                (oldContent as TITS.Library.Song).NowPlayingStateChanged -= HandleContentNowPlayingStateChanged;
            }

            base.OnContentChanged(oldContent, newContent);

            if (newContent != null)
            {
                (newContent as TITS.Library.Song).NowPlayingStateChanged += HandleContentNowPlayingStateChanged; 
            }
        }

        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive", typeof(bool), typeof(SongItem), new PropertyMetadata(false));

        public bool IsActive
        {
            get
            {
                return (bool)GetValue(IsActiveProperty);
            }
            set
            {
                Dispatcher.Invoke(() =>
                {
                    SetValue(IsActiveProperty, value);
                });
            }
        }

        public string SongNo
        {
            get
            {
                return (this.Content as TITS.Library.Song).Metadata.Track.ToString();
            }
        }

        public string SongTitle
        {
            get
            {
                return (this.Content as TITS.Library.Song).Metadata.Title.ToString();
            }
        }

        private void HandleContentNowPlayingStateChanged(object sender, EventArgs e)
        {
            IsActive = (sender as TITS.Library.Song).NowPlaying;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion
    }
}
