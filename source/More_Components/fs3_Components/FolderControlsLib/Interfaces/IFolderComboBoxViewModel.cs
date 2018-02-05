namespace FolderControlsLib.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Input;
    using FileSystemModels.Events;
    using FileSystemModels.Interfaces;

    /// <summary>
    /// Defines an interface to a viewmodel that can be used for a
    /// combobox that browses to different folder locations.
    /// </summary>
    public interface IFolderComboBoxViewModel
    {
        #region properties
        /// <summary>
        /// Event is fired when the path in the text portion of the combobox
        /// is changed. Client applications should listen to this event to
        /// find out when the user indicates a change of the file system
        /// location through the bound control.
        /// </summary>
        event EventHandler<FolderChangedEventArgs> RequestChangeOfDirectory;

        /// <summary>
        /// Expose a collection of file system items (folders and hard disks and ...) that
        /// can be selected and navigated to in this viewmodel.
        /// </summary>
        IEnumerable<IFolderItemViewModel> CurrentItems { get; }

        /// <summary>
        /// Gets a command that should be invoked when the combobox view tells
        /// the viewmodel that the current path selection has changed
        /// (via selection changed event or keyup events).
        /// 
        /// The parameter p can be an array of objects
        /// containing objects of the FSItemVM type or
        /// p can also be string.
        /// 
        /// Each parameter item that adheres to the above types results in
        /// a OnCurrentPathChanged event being fired with the folder path
        /// as parameter.
        /// </summary>
        ICommand SelectionChanged { get; }

        /// <summary>
        /// Gets/sets the currently selected file system viewmodel item.
        /// </summary>
        IFolderItemViewModel SelectedItem { get; }

        /// <summary>
        /// Get/sets viewmodel data pointing at the path
        /// of the currently selected folder.
        /// </summary>
        string CurrentFolder { get; }

        /// <summary>
        /// Gets a string that can be displayed as a tooltip for the
        /// viewmodel data pointing at the path of the currently selected folder.
        /// </summary>
        string CurrentFolderToolTip { get; }
        #endregion properties

        #region methods
        /// <summary>
        /// Can be invoked to refresh the currently visible set of data.
        /// </summary>
        void PopulateView(IPathModel newPath);
        #endregion methods
    }
}