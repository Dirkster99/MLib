namespace HistoryControlLib.Interfaces
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines an interface to an object that implements a navigational list of
    /// recent locations, which were recently visited, and may be suggested for
    /// re-visitation.
    /// 
    /// The object that implements this interface can manage recently visited locations and supports:
    /// 
    /// - adding more recently visited locations
    /// - forward and backward navigation, and
    /// - selection of any position within the given set of locations.
    /// </summary>
    public interface IBrowseHistory<T>
    {
        #region properties
        /// <summary>
        /// Gets a current visiting location (if any) or
        /// -1 if there is no current location available.
        /// </summary>
        int SelectedIndex { get; }

        /// <summary>
        /// Gets a list of recently visited locations.
        /// </summary>
        IEnumerable<T> Locations { get; }

        /// <summary>
        /// Gets the size of the currently available list of locations.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets the currently selected item or default(t) (usually null)
        /// if there is no currently selected item.
        /// </summary>
        T SelectedItem { get; }

        /// <summary>
        /// Determines if backward navigation is possible (returns true)
        /// (based on set of locations and SelectedIndex) or not (returns false).
        /// </summary>
        bool CanBackward { get; }

        /// <summary>
        /// Determines if forward navigation within the current set of locations
        /// is possible (returns true) - based on set of locations and SelectedIndex
        /// or not (returns false).
        /// </summary>
        bool CanForward { get; }

        /// <summary>
        /// Gets the item that would be selected next if we where to navigate back to the
        /// next item (if any) in the current list of recent locations.
        /// </summary>
        T NextBackWardItem { get; }
        #endregion properties

        #region methods
        /// <summary>
        /// Navigates backward in the list of currently available locations.
        ///
        /// Returns false if backward navigation is possible or true, otherwise.
        /// </summary>
        bool Backward();

        /// <summary>
        /// Removes all currently logged locations and sets:
        /// <seealso cref="SelectedIndex"/> = -1
        /// </summary>
        void ClearLocations();

        /// <summary>
        /// Navigates forward within the current set of locations (without adding a location)
        /// and returns true if this is possible - based on set of locations and SelectedIndex
        /// or false, if navigation forward is not possible.
        /// </summary>
        bool Forward();

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
        void Forward(T newLocation);

        /// <summary>
        /// Set the <seealso cref="SelectedIndex"/> property within the currently
        /// available collection of locations or throws an exception
        /// if the requested index is out of bounds.
        /// </summary>
        void SetSelectedIndex(int idx);
        #endregion methods
    }
}