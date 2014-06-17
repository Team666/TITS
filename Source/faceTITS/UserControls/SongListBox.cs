using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using System.ComponentModel;

namespace faceTITS.UserControls
{
    class SongListBox : System.Windows.Controls.ListBox
    {

        // Kinda sorta singleton DIRTY HACK NO SCOPE ~~
        static SongListBox control;
        public void SetStaticRefToSelf()
        {
            if (control == null)
            {
                control = this;
            }
        }

        public SongListBox()
        {
        }

        public static readonly DependencyProperty ActiveItemProperty = 
            DependencyProperty.RegisterAttached("ActiveItem", typeof(int), typeof(SongListBox), new PropertyMetadata(-1, onActiveItemChanged));

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is SongItem;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new SongItem();
        }

        public int ActiveItem
        {
            set
            {
                SetValue(ActiveItemProperty, value);

                /*if (_activeItem > -1)
                {
                   (this.Items[_activeItem] as SongItem).IsActive = false;
                }

                (this.Items[value] as SongItem).IsActive = true;*/
            }

            get
            {
                return (int)GetValue(ActiveItemProperty);
            }
        }

        public static void onActiveItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var oldValue = (int)e.OldValue;
            var newValue = (int)e.NewValue;

            if (control == null)
            {
                return;
            }

            if (oldValue > -1)
            {
                var oldThingy = (control.ItemContainerGenerator.ContainerFromIndex(oldValue) as SongItem);
                oldThingy.IsActive = false;
            }

            var newThingy = (control.ItemContainerGenerator.ContainerFromIndex(newValue) as SongItem);
            newThingy.IsActive = true;
        }
    }

    class SongItem : System.Windows.Controls.ListBoxItem, INotifyPropertyChanged
    {
        public SongItem() : base()
        {       
        }

        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register("IsActive", typeof(bool), typeof(SongItem), new PropertyMetadata(false));

        public bool IsActive
        {
            get
            {
                return (bool)GetValue(IsActiveProperty);
            }
            set
            {
                SetValue(IsActiveProperty, value);
            }
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
