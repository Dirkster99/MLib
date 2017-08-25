namespace FsContentDialogDemo.Demos.ViewModels
{
    using FolderBrowser;
    using FolderBrowser.Dialogs.Interfaces;
    using FsContentDialogDemo.ViewModels.Base;
    using System.Windows.Input;

    public class DemoViewModel : MWindowDialogLib.ViewModels.Base.BaseViewModel
    {
        #region private fields
        private string _Path;
        private ICommand _ShowConententDialogCommand;
        private IBookmarkedLocationsViewModel _BookmarkedLocation;
        #endregion private fields

        #region constructors
        /// <summary>
        /// Class constructor
        /// </summary>
        public DemoViewModel()
        {
            _Path = @"C:\Program Files\Microsoft SQL Server\90\Shared\Resources\1028";
            BookmarkedLocations = ConstructBookmarks();

            BookmarkedLocations.RequestChangeOfDirectory += BookmarkedLocations_RequestChangeOfDirectory;
        }
        #endregion constructors

        #region properties
        /// <summary>
        /// Gets/sets a bookmark folder property to manage bookmarked folders.
        /// </summary>
        public IBookmarkedLocationsViewModel BookmarkedLocations
        {
            get
            {
                return _BookmarkedLocation;
            }

            private set
            {
                if (_BookmarkedLocation != value)
                {
                    _BookmarkedLocation = value;
                    RaisePropertyChanged(() => BookmarkedLocations);
                }
            }
        }

        /// <summary>
        /// Gets/sezs a string representing a path to demo ...
        /// </summary>
        public string Path
        {
            get { return _Path; }
            set
            {
                if (_Path != value)
                {
                    _Path = value;
                    RaisePropertyChanged(() => this.Path);
                }
            }
        }

        public ICommand ShowConententDialogCommand
        {
            get
            {
                if (_ShowConententDialogCommand == null)
                {
                    _ShowConententDialogCommand = new RelayCommand<object>(async (p) =>
                    {
                        var initialPath = p as string;

                        if (string.IsNullOrEmpty(initialPath) == true)
                            initialPath = this.GetDefaultPath(FsContentDialogDemo.ViewModels.AppLifeCycleViewModel.MyDocumentsUserDir);

                        var bookmark = FolderBrowser.FolderBrowserFactory.CreateReceentLocationsViewModel();
                        bookmark.AddFolder(@"c:\windows");

                        FolderBrowserControler progressDlgDemo = new FolderBrowserControler(
                            initialPath, bookmark);

                        var path = await progressDlgDemo.ShowContentDialogFromVM(this, true);

                        if (string.IsNullOrEmpty(path) == false)
                            this.Path = path;
                    },
                    (p) => { return true; });
                }

                return _ShowConententDialogCommand;
            }
        }
        #endregion properties

        #region methods
        /// <summary>
        /// Get a source or distination file path as default path
        /// </summary>
        /// <param name="defaultPath"></param>
        /// <returns></returns>
        private string GetDefaultPath(string defaultPath = @"C:\")
        {
            // Insert Appplication Specific default paths here ...

            return defaultPath;
        }

        /// <summary>
        /// Constructs a few initial entries for
        /// the recent folder collection that implements folder bookmarks.
        /// </summary>
        /// <returns></returns>
        private IBookmarkedLocationsViewModel ConstructBookmarks()
        {
            IBookmarkedLocationsViewModel ret = FolderBrowserFactory.CreateReceentLocationsViewModel();

            ret.AddFolder(@"C:\Windows");
            ret.AddFolder(@"C:\Users");
            ret.AddFolder(@"C:\Program Files");

            ret.SelectedItem = ret.DropDownItems[0];

            return ret;
        }

        /// <summary>
        /// Method is invoked when the user clicks the drop down and has selected an item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BookmarkedLocations_RequestChangeOfDirectory(object sender, FolderBrowser.Events.FolderChangedEventArgs e)
        {
            this.Path = e.Folder.Path;
        }
        #endregion methods
    }
}
