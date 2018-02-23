namespace FileSystemModels.Interfaces
{
    using FileSystemModels.Models.FSItems.Base;

    /// <summary>
    /// Define the properties and methods of a viewmodel for
    /// a file system item.
    /// </summary>
    public interface IListItemViewModel
    {
        #region properties
        /// <summary>
        /// Gets the type (folder, file) of this item
        /// </summary>
        FSItemType ItemType { get; }

        /// <summary>
        /// Gets the path to this item
        /// </summary>
        string FullPath { get; }

        /// <summary>
        /// Gets a name that can be used for display
        /// (is not necessarily the same as path)
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// Gets the type (folder, file) of this item
        /// </summary>
        IPathModel GetModel { get; }

        /// <summary>
        /// Gets whether or not to show an Icon for this item or not.
        /// </summary>
        bool ShowIcon { get; }
        #endregion properties

        #region methods
        /// <summary>
        /// Determine whether a given path is an exeisting directory or not.
        /// </summary>
        /// <returns>true if this directory exists and otherwise false</returns>
        bool DirectoryPathExists();

        /// <summary>
        /// Gets a folder item string for display purposes.
        /// This string can evaluete to 'C:\ (Windows)' for drives,
        /// if the 'C:\' drive was named 'Windows'.
        /// </summary>
        string DisplayItemString();
        #endregion methods
    }
}
