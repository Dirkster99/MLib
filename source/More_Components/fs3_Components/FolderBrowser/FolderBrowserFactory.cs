namespace FolderBrowser
{
    using FolderBrowser.Dialogs.Interfaces;
    using FolderBrowser.Dialogs.ViewModels;
    using FolderBrowser.Interfaces;
    using FolderBrowser.ViewModels;
    using MWindowInterfacesLib.MsgBox;

    /// <summary>
    /// Implements a factory for generating internal classes that are otherwise
    /// accessible through public interface definitions only. Use this factory to generate
    /// the main classes and work with their properties and methods that are accessible
    /// through their related interface definitions.
    /// </summary>
    public static class FolderBrowserFactory
    {
        /// <summary>
        /// Create a dropdown viewmodel object that can be used to manage a dropdown view
        /// that contains a browser naviagtion (directory picker) control.
        /// </summary>
        /// <param name="treeBrowser"></param>
        /// <param name="recentLocations"></param>
        /// <param name="resultCallback"></param>
        /// <returns></returns>
        public static IDropDownViewModel CreateDropDownViewModel(
            IMessageBoxService msgBox,
            IBrowserViewModel treeBrowser,
            IBookmarkedLocationsViewModel recentLocations,
            DropDownClosedResult resultCallback)
        {
            return new DropDownViewModel(msgBox, treeBrowser, recentLocations, resultCallback);
        }

        /// <summary>
        /// Create a browser viewmodel object that can be used to manage a
        /// browser naviagtion (directory picker) control.
        /// </summary>
        /// <param name="msgBox">Reference to a mssage box service for
        /// displaying messages to the user</param>
        /// <returns></returns>
        public static IBrowserViewModel CreateBrowserViewModel(
             IMessageBoxService msgBox
            ,bool specialFolderVisibility = true
            ,string initialPath = null)
        {
            BrowserViewModel treeBrowserVM = null;

            if (initialPath != null)
                treeBrowserVM = new BrowserViewModel(msgBox) { InitialPath = initialPath };
            else
                treeBrowserVM = new BrowserViewModel(msgBox);

            treeBrowserVM.SetSpecialFoldersVisibility(specialFolderVisibility);

            return treeBrowserVM;
        }

        /// <summary>
        /// Create a dialog viewmodel object that can be used to manage a dialog
        /// that contains a browser naviagtion (directory picker) control.
        /// </summary>
        /// <param name="treeBrowser"></param>
        /// <param name="recentLocations"></param>
        /// <returns></returns>
        public static IDialogViewModel CreateDialogViewModel(
            IMessageBoxService msgBox,
            IBrowserViewModel treeBrowser = null,
            IBookmarkedLocationsViewModel recentLocations = null)
        {
            return new DialogViewModel(msgBox, treeBrowser, recentLocations);
        }

        /// <summary>
        /// Factory pattern that can create objects to manage
        /// recently visited file system folder entries.
        /// </summary>
        /// <returns></returns>
        public static IBookmarkedLocationsViewModel CreateReceentLocationsViewModel()
        {
            return new BookmarkedLocationsViewModel() as IBookmarkedLocationsViewModel;
        }
    }
}
