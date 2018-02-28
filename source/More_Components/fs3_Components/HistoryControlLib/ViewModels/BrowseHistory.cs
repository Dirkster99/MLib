namespace HistoryControlLib.ViewModels
{
    using HistoryControlLib.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;

    /// <summary>
    /// Implements a navigational list of recent locations which were recently
    /// visited and may be suggested for re-visitation.
    /// </summary>
    internal class BrowseHistory<T> : Base.BaseViewModel, IBrowseHistory<T>
    {
        #region fields
        const int ListLimit = 128;

        private readonly ObservableCollection<T> _Locations;
        private int _SelectedIndex = -1;
        #endregion fields

        #region ctors
        /// <summary>
        /// Class constructor
        /// </summary>
        public BrowseHistory()
        {
            _Locations = new ObservableCollection<T>();
        }
        #endregion ctors

        #region properties
        /// <summary>
        /// Gets a list of recently visited locations.
        /// </summary>
        public IEnumerable<T> Locations
        {
            get
            {
                return _Locations;
            }
        }

        /// <summary>
        /// Gets the size of the currently available list of locations.
        /// </summary>
        public int Count
        {
            get
            {
                return _Locations.Count;
            }
        }

        /// <summary>
        /// Gets a current visiting location (if any) or
        /// -1 if there is no current location available.
        /// </summary>
        public int SelectedIndex
        {
            get
            {
                return _SelectedIndex;
            }

            private set
            {
                if (_SelectedIndex != value)
                {
                    _SelectedIndex = value;
                    NotifyPropertyChanged(() => SelectedIndex);
                    NotifyPropertyChanged(() => SelectedItem);

                    NotifyPropertyChanged(() => CanBackward);
                    NotifyPropertyChanged(() => CanForward);
                }
            }
        }

        /// <summary>
        /// Gets the currently selected item or default(t) (usually null)
        /// if there is no currently selected item.
        /// </summary>
        public T SelectedItem
        {
            get
            {
                if (_SelectedIndex >= 0 && _SelectedIndex <_Locations.Count)
                    return _Locations[_SelectedIndex];

                return default(T);
            }
        }

        /// <summary>
        /// Gets the item that would be selected next if we where to navigate back to the
        /// next item (if any) in the current list of recent locations.
        /// </summary>
        public T NextBackWardItem
        {
            get
            {
                if (CanBackward == false)
                    return default(T);

                return _Locations[SelectedIndex + 1];
            }
        }

        /// <summary>
        /// Determines if forward navigation within the current set of locations
        /// is possible (returns true) - based on set of locations and SelectedIndex
        /// or not (returns false).
        /// </summary>
        public bool CanForward
        {
            get
            {
                if (SelectedIndex > 0)
                    return true;

                return false;
            }
        }

        /// <summary>
        /// Determines if backward navigation is possible (returns true)
        /// (based on set of locations and SelectedIndex) or not (returns false).
        /// </summary>
        public bool CanBackward
        {
            get
            {
                if ((SelectedIndex + 1) < _Locations.Count)
                    return true;

                return false;
            }
        }
        #endregion properties

        #region methods
        /// <summary>
        /// Removes all currently logged locations and sets:
        /// <seealso cref="SelectedIndex"/> = -1
        /// </summary>
        public void ClearLocations()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                _Locations.Clear();
                SelectedIndex = -1;
            }));
        }

        /// <summary>
        /// Set the <seealso cref="SelectedIndex"/> property within the currently
        /// available collection of locations or throws an exception
        /// if the requested index is out of bounds.
        /// </summary>
        public void SetSelectedIndex(int idx)
        {
            if (idx < 0 || (idx + 1) > _Locations.Count)
                throw new System.ArgumentOutOfRangeException(string.Format("Index {0} out of bounds between 0 and {1}", idx, _Locations.Count));

            SelectedIndex = idx;
        }

        /// <summary>
        /// Navigates forward in the list of currently available locations.
        ///
        /// The implemented behavior depends on the current set of locations
        /// and the selected element within that set of locations.
        ///
        /// The method adds the new item at index 0 if the currently selected item
        /// has the index 0 or if the current list of locations is empty (SelectedIndex = -1).
        ///
        /// The new item is inserted at SelectedIndex -1 and the SelectedIndex is set to that item.
        /// All items before the new item are removed.
        ///
        /// All items with an index greater limit n are removed,
        /// if the list gets larger than limit n.
        /// </summary>
        public void Forward(T newLocation)
        {
            if (SelectedIndex >= 0)
            {
                var equi = newLocation as IEqualityComparer<T>;
                if (equi != null)
                {
                    // Do nothing if a forward on the current location appears
                    // to describe the requested location
                    if (equi.Equals(newLocation, _Locations[SelectedIndex]) == true)
                        return;
                }
            }

            if (SelectedIndex > 0)
            {
                // Update this item to recycle spot in list
                ReplaceLocationAt(SelectedIndex - 1, newLocation);

                for (int i = 0; i < (SelectedIndex - 1); i++) // Remove all previous items
                    RemoveLocationAt(0);

                SelectedIndex = 0;                          // Reset current position
            }
            else // Just insert at beginning of list
            {
                _SelectedIndex = -1;                   // Reset this to enforce notification on
                InsertLocationAt(0, newLocation);     // SelectedIndex = 0;
                SelectedIndex = 0;
            }

            if (_Locations.Count > ListLimit)        // Make sure list cannot grow beyond useful size
            {
                int delta = _Locations.Count - ListLimit;

                for (int i = 0; i < delta; i++)
                {
                    RemoveLocationAt(_Locations.Count - 1);  // Always remove last element
                }
            }

            NotifyPropertyChanged(() => CanBackward);
            NotifyPropertyChanged(() => CanForward);
        }

        /// <summary>
        /// Navigates forward within the current set of locations (without adding a location)
        /// and returns true if this is possible - based on set of locations and SelectedIndex
        /// or false, if navigation forward is not possible.
        /// </summary>
        public bool Forward()
        {
            if (SelectedIndex > 0)
            {
                SelectedIndex--;

                NotifyPropertyChanged(() => CanBackward);
                NotifyPropertyChanged(() => CanForward);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Navigates backward in the list of currently available locations.
        ///
        /// Returns false if backward navigation is possible or true, otherwise.
        /// </summary>
        public bool Backward()
        {
            if ((SelectedIndex + 1) < _Locations.Count)
            {
                SelectedIndex++;

                NotifyPropertyChanged(() => CanBackward);
                NotifyPropertyChanged(() => CanForward);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Implements standard override for ToString() method.
        /// </summary>
        public override string ToString()
        {
            string ret = string.Empty;

            for (int i = 0; i < _Locations.Count; i++)
            {
                ret += string.Format("{0:00} ", i) + _Locations[i].ToString();

                if (i == SelectedIndex)
                    ret += " CURRENT POSITION";

                ret += '\n';
            }

            ret += "\n\n" + string.Format(" CurrenPosition : {0:00}", SelectedIndex) + '\n';

            return ret;
        }

        /// <summary>
        /// Updates the item with index <paramref name="pos"/> to recycle spot
        /// in list with item <paramref name="item"/> (old item is replaced with new item).
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="item"></param>
        private void ReplaceLocationAt(int pos, T item)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                _Locations[pos] = item;   // Update this item to recycle spot in list
            }));
        }

        private void InsertLocationAt(int pos, T item)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                _Locations.Insert(pos, item);    // SelectedIndex = 0;
            }));
        }

        private void RemoveLocationAt(int pos)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                _Locations.RemoveAt(pos);    // SelectedIndex = 0;
            }));
        }
        #endregion methods
    }
}