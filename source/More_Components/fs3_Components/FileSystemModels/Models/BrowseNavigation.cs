namespace FileSystemModels.Models
{
    using System.Collections.Generic;
    using System.IO;
    using FileSystemModels.Interfaces;
    using FileSystemModels.Models.FSItems.Base;

    /// <summary>
    /// Class implements a model that keeps track of a browsing history similar to the well known
    /// Internet browser functionality that lets users go forward and backward between visited URLs.
    /// </summary>
    public class BrowseNavigation : IBrowseNavigation
    {
        #region fields
        /// <summary>
        /// Log4Net instance for logging errors, warnings etc
        /// </summary>
        protected static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Defines the delimitor for multiple regular expression filter statements.
        /// eg: "*.txt;*.ini"
        /// </summary>
        private const char FilterSplitCharacter = ';';

        /// <summary>
        /// Determines whether the redo stack (FutureFolders) should be cleared when the
        /// CurrentFolder changes next time
        /// </summary>
        private string mFilterString = string.Empty;
        #endregion fields

        #region constructor
        /// <summary>
        /// Class constructor
        /// </summary>
        public BrowseNavigation()
        {
            this.PastLocations = new Stack<IPathModel>();
            this.FutureLocations = new Stack<IPathModel>();
        }
        #endregion constructor

        #region properties
        /// <summary>
        /// Gets/sets the current folder which is being
        /// queried to list the current files and folders for display.
        /// </summary>
        public IPathModel CurrentFolder { get; private set; }

        /// <summary>
        /// Gets/sets the undo stacks for navigation.
        /// </summary>
        private Stack<IPathModel> PastLocations { get; }

        /// <summary>
        /// Gets/sets the redo stack for navigation.
        /// </summary>
        private Stack<IPathModel> FutureLocations { get; }
        #endregion properties

        #region methods
        /// <summary>
        /// Converts a filter string from "*.txt;*.tex" into a
        /// string array based format string[] filterString = { "*.txt", "*.tex" };
        /// </summary>
        /// <param name="inputFilterString"></param>
        public static string[] GetParsedFilters(string inputFilterString)
        {
            string[] filterString = { "*" };

            try
            {
                if (string.IsNullOrEmpty(inputFilterString) == false)
                {
                    if (inputFilterString.Split(BrowseNavigation.FilterSplitCharacter).Length > 1)
                        filterString = inputFilterString.Split(BrowseNavigation.FilterSplitCharacter);
                    else
                    {
                        // Add asterix at front and beginning if user is too non-technical to type it.
                        if (inputFilterString.Contains("*") == false)
                            filterString = new string[] { "*" + inputFilterString + "*" };
                        else
                            filterString = new string[] { inputFilterString };
                    }
                }
            }
            catch
            {
            }

            return filterString;
        }

        /// <summary>
        /// Navigates to a previously visited folder (if any).
        /// </summary>
        IPathModel IBrowseNavigation.BrowseBack()
        {
            try
            {
                if (PastLocations.Count > 0)
                {
                    // top of stack is always last location
                    if (CurrentFolder != null)
                        FutureLocations.Push(CurrentFolder);

                    CurrentFolder = PastLocations.Pop();

                    return CurrentFolder.Clone() as IPathModel;
                }
            }
            catch
            {
            }

            return null;
        }

        /// <summary>
        /// Determines whether the model can navigate to a previously visited folder or not.
        /// </summary>
        /// <returns></returns>
        bool IBrowseNavigation.CanBrowseBack()
        {
            return this.PastLocations.Count > 0;
        }

        /// <summary>
        /// Navigates to a folder that was visited before navigating back (if any).
        /// </summary>
        IPathModel IBrowseNavigation.BrowseForward()
        {
            if (this.FutureLocations.Count > 0)
            {
                bool pushPastLocation = true;

                if (this.CurrentFolder == null)
                    pushPastLocation = false;
                else
                {
                    if (PastLocations.Count > 0) // Don't push same location twice on top
                    {
                        if (PathModel.Compare(this.PastLocations.Peek(), this.CurrentFolder) == true)
                            pushPastLocation = false;
                    }
                }

                if (pushPastLocation == true)
                    this.PastLocations.Push(this.CurrentFolder);

                this.CurrentFolder = this.FutureLocations.Pop();

                return this.CurrentFolder.Clone() as IPathModel;
            }

            return null;
        }

        /// <summary>
        /// Determine if navigates to a folder that was visited before navigating
        /// back is currently possible or not.
        /// </summary>
        bool IBrowseNavigation.CanBrowseForward()
        {
            return this.FutureLocations.Count > 0;
        }

        /// <summary>
        /// Browse into the parent folder location of a given path.
        /// </summary>
        IPathModel IBrowseNavigation.BrowseUp()
        {
            string[] dirs = PathModel.GetDirectories(this.CurrentFolder.Path);

            if (dirs.Length > 1)
            {
                string newf = string.Join(System.IO.Path.DirectorySeparatorChar.ToString(), dirs, 0, dirs.Length - 1);

                var newDir = new PathModel(newf, FSItemType.Folder);

                SetCurrentFolder(newDir.Path, true);

                return newDir.Clone() as IPathModel;
            }

            return null;
        }

        /// <summary>
        /// Determine whether browsing into the parent folder
        /// path of a given path is possible or not.
        /// </summary>
        bool IBrowseNavigation.CanBrowseUp()
        {
            if (this.CurrentFolder == null)
            {
                Logger.Warn("CurrentFolder is (null) but it should always be available.");

                return false;
            }

            string[] dirs = PathModel.GetDirectories(this.CurrentFolder.Path);

            if (dirs != null)
                return dirs.Length > 1;

            return false;
        }

        /// <summary>
        /// Method is executed when a listview item is double clicked.
        /// </summary>
        /// <param name="infoType"></param>
        /// <param name="newPath"></param>
        FSItemType IBrowseNavigation.BrowseDown(FSItemType infoType, string newPath)
        {
            if (infoType == FSItemType.Folder || infoType == FSItemType.LogicalDrive)
                SetCurrentFolder(newPath, true);

            return infoType;
        }

        /// <summary>
        /// Determines whether the current path represents a directory or not.
        /// </summary>
        /// <returns>true if directory otherwise false</returns>
        bool IBrowseNavigation.IsCurrentPathDirectory()
        {
            if (CurrentFolder != null)
                return (CurrentFolder.DirectoryPathExists());

            return false;
        }

        /// <summary>
        /// Get the file system object that represents the current directory.
        /// </summary>
        /// <returns></returns>
        DirectoryInfo IBrowseNavigation.GetDirectoryInfoOnCurrentFolder()
        {
            try
            {
                return new DirectoryInfo(this.CurrentFolder.Path);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets an array of the current filter items eg: "*.txt", "*.pdf", "*.doc"
        /// </summary>
        /// <returns></returns>
        string[] IBrowseNavigation.GetFilterArray()
        {
            string[] filterString = { "*.*" };

            if (string.IsNullOrEmpty(this.mFilterString) == false)
            {
                if (this.mFilterString.Split(BrowseNavigation.FilterSplitCharacter).Length > 1)
                    filterString = this.mFilterString.Split(BrowseNavigation.FilterSplitCharacter);
                else
                    filterString = new string[] { this.mFilterString };
            }

            return filterString;
        }

        /// <summary>
        /// Resets the current filter string in raw format.
        /// </summary>
        /// <param name="filterText"></param>
        void IBrowseNavigation.SetFilterString(string filterText)
        {
            this.mFilterString = filterText;
        }

        /// <summary>
        /// Sets the current folder to a new folder (with or without adjustments of History).
        /// </summary>
        /// <param name="path"></param>
        /// <param name="bSetHistory"></param>
        public void SetCurrentFolder(string path, bool bSetHistory)
        {
            if (bSetHistory == true)
            {
                bool pushRecentFolder = true;

                if (this.CurrentFolder == null)
                    pushRecentFolder = false;
                else
                {
                    // Do not set the same location twice
                    if (this.PastLocations.Count > 0) // Don't push same folder twice
                    {
                        if (PathModel.Compare(this.PastLocations.Peek(), this.CurrentFolder) == true)
                            pushRecentFolder = false;
                        else
                            pushRecentFolder = true;
                    }
                    else      // Past is empty so what ever we have now is being recorded
                        pushRecentFolder = true;
                }

                if (pushRecentFolder == true)
                    this.PastLocations.Push(this.CurrentFolder);

                this.FutureLocations.Clear();
                this.CurrentFolder = new PathModel(path, FSItemType.Folder);
            }
            else
                this.CurrentFolder = new PathModel(path, FSItemType.Folder);
        }
        #endregion methods
    }
}
