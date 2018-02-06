namespace FileSystemModels.Browse
{
    using FileSystemModels.Interfaces;
    using System;
    using System.Threading.Tasks;

    public interface INavigateable
    {
        /// <summary>
        /// Indicates when the viewmodel starts heading off somewhere else
        /// and when its done browsing to a new location.
        /// </summary>
        event EventHandler<BrowsingEventArgs> BrowseEvent;

        // Can only be set by the control is user started browser process
        bool IsBrowsing { get; }

        // Controller can start browser process if IsBrowsing = false
        bool NavigateTo(IPathModel newPath);

        // Controller can start browser process if IsBrowsing = false
        Task<bool> NavigateToAsync(IPathModel newPath);

        // Can only be set by the controller if browser process was started externally
        void SetExternalBrowsingState(bool isBrowsing);
    }
}
