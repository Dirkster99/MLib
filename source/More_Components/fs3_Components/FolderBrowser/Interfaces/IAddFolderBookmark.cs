namespace FolderBrowser.Interfaces
{
    using FileSystemModels.Events;
    using System;
    using System.Windows.Input;

    /// <summary>
    /// Defines an interface to Add/Remove Bookmark entries from
    /// a list of bookmarked items.
    /// </summary>
    public interface IAddFolderBookmark
    {
        event EventHandler<RecentFolderEvent> RequestEditBookmarkedFolders;

        ICommand RecentFolderAddCommand { get; }
    }
}
