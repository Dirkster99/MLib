namespace MDemo.Demos.fsc
{
    using FolderBrowser;
    using FolderBrowser.Dialogs.Interfaces;
    using MDemo.ViewModels.Base;
    using MWindowInterfacesLib.Interfaces;
    using System.Windows.Input;

    public class FolderBrowserViewModel : ViewModelBase
    {
        #region fields
        private ICommand mSelectFolderCommand = null;
        private string mInitialPath = @"C:\";
        private IBookmarkedLocationsViewModel mBookmarkedLocation;
        private IDropDownViewModel mDropDownBrowser = null;
        #endregion fields

        #region constructors
        public FolderBrowserViewModel()
        {
            InitialPath = @"C:\Program Files\MSBuild\Microsoft\Windows Workflow Foundation\v3.0";
            BookmarkedLocations = this.ConstructBookmarks();

            DropDownBrowser = InitializeDropDownBrowser(InitialPath);
        }
        #endregion constructors

        #region properties
        /// <summary>
        /// Gets/sets a string property to determine the initial path
        /// to browse to upon oping the folder browser dialog.
        /// </summary>
        public string InitialPath
        {
            get
            {
                return mInitialPath;
            }

            set
            {
                if (mInitialPath != value)
                {
                    mInitialPath = value;
                    RaisePropertyChanged(() => InitialPath);
                }
            }
        }

        /// <summary>
        /// Gets a viewmodel object that can be used to drive a folder browser
        /// displayed inside a drop down button element.
        /// </summary>
        public IDropDownViewModel DropDownBrowser
        {
            get
            {
                return mDropDownBrowser;
            }

            private set
            {
                if (mDropDownBrowser != value)
                {
                    mDropDownBrowser = value;
                    RaisePropertyChanged(() => DropDownBrowser);
                }
            }
        }

        /// <summary>
        /// Gets/sets a bookmark folder property to manage bookmarked folders.
        /// </summary>
        public IBookmarkedLocationsViewModel BookmarkedLocations
        {
            get
            {
                return mBookmarkedLocation;
            }

            private set
            {
                if (mBookmarkedLocation != value)
                {
                    mBookmarkedLocation = value;
                    RaisePropertyChanged(() => BookmarkedLocations);
                }
            }
        }

        /// <summary>
        /// Gets a command to demo the folder browser dialog...
        /// </summary>
        public ICommand SelectFolderCommand
        {
            get
            {
                if (mSelectFolderCommand == null)
                {
                    mSelectFolderCommand = new RelayCommand<object>((p) =>
                    {
                        var msgBox = GetService<IContentDialogService>().MsgBox;

                        var dlg = new FolderBrowser.Views.FolderBrowserDialog();

                        // See Loaded event in FolderBrowserTreeView_Loaded methid to understand initial load
                        var treeBrowser = FolderBrowserFactory.CreateBrowserViewModel(msgBox);

                        // Switch updates to view of by default to speed up load of view
                        // Loading the view will kick-off the browsing via View.Loaded Event
                        // and that in turn will switch on view updates ...
                        treeBrowser.UpdateView = false;

                        var initialPath = p as string;

                        if (initialPath != null)
                            treeBrowser.InitialPath = initialPath;
                        else
                            treeBrowser.InitialPath = this.InitialPath;

                        treeBrowser.SetSpecialFoldersVisibility(true);

                        var dlgVM = FolderBrowserFactory.CreateDialogViewModel(
                            msgBox, treeBrowser, BookmarkedLocations);

                        dlg.DataContext = dlgVM;

                        bool? bResult = dlg.ShowDialog();

                        if (dlgVM.DialogCloseResult == true || bResult == true)
                        {
                            InitialPath = dlgVM.TreeBrowser.SelectedFolder;

                            if (dlgVM.BookmarkedLocations != null)
                                this.BookmarkedLocations = dlgVM.BookmarkedLocations.Copy();
                        }
                    });
                }

                return mSelectFolderCommand;
            }
        }
        #endregion properties

        #region methods
        /// <summary>
        /// Constructs a few initial entries for
        /// the recent folder collection that implements folder bookmarks.
        /// </summary>
        /// <returns></returns>
        private IBookmarkedLocationsViewModel ConstructBookmarks()
        {
            IBookmarkedLocationsViewModel ret = FolderBrowserFactory.CreateReceentLocationsViewModel();

            ret.AddFolder(@"C:\Windows");
            ret.AddFolder(@"C:\Temp");

            ret.SelectedItem = ret.DropDownItems[0];

            return ret;
        }

        private string UpdateCurrentPath()
        {
            return this.InitialPath;
        }

        private IBookmarkedLocationsViewModel UpdateBookmarks()
        {
            return this.BookmarkedLocations;
        }

        /// <summary>
        /// Method configures a drop down element to show a
        /// folder picker dialog on opening up.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private IDropDownViewModel InitializeDropDownBrowser(string path)
        {
            var msgBox = GetService<IContentDialogService>().MsgBox;

            // See Loaded event in FolderBrowserTreeView_Loaded method to understand initial load
            var treeBrowser = FolderBrowserFactory.CreateBrowserViewModel(msgBox);

            // Switch updates to view of by default to speed up load of view
            // Loading the view will kick-off the browsing via View.Loaded Event
            // and that in turn will switch on view updates ...
            treeBrowser.UpdateView = false;

            if (string.IsNullOrEmpty(path) == false)
                treeBrowser.InitialPath = path;
            else
                treeBrowser.InitialPath = this.InitialPath;

            treeBrowser.SetSpecialFoldersVisibility(true);

            var dlgVM = FolderBrowserFactory.CreateDropDownViewModel(
                msgBox, treeBrowser, BookmarkedLocations, this.DropDownClosedResult);

            dlgVM.UpdateInitialPath = this.UpdateCurrentPath;
            dlgVM.UpdateInitialBookmarks = this.UpdateBookmarks;

            dlgVM.ButtonLabel = "Select a Folder";

            return dlgVM;
        }

        /// <summary>
        /// Method is invoked when drop element is closed.
        /// </summary>
        /// <param name="bookmarks"></param>
        /// <param name="selectedPath"></param>
        /// <param name="result"></param>
        private void DropDownClosedResult(IBookmarkedLocationsViewModel bookmarks,
                                          string selectedPath,
                                          FolderBrowser.Dialogs.Interfaces.Result result)
        {
            if (result == FolderBrowser.Dialogs.Interfaces.Result.OK)
            {
                if (bookmarks != null)
                    this.BookmarkedLocations = bookmarks.Copy();

                if (string.IsNullOrEmpty(selectedPath) == false)
                    this.InitialPath = selectedPath;
                else
                    this.InitialPath = @"C:\\";
            }
        }
        #endregion methods
    }
}
