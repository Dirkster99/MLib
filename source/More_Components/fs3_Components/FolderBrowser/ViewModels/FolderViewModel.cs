namespace FolderBrowser.ViewModels
{
    using FileSystemModels.Models;
    using FileSystemModels.Models.FSItems;
    using FolderBrowser.Interfaces;
    using InplaceEditBoxLib.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Implment the viewmodel for one folder entry for a collection of folders.
    /// </summary>
    internal class FolderViewModel : EditInPlaceViewModel, IFolderViewModel
    {
        #region fields
        private static readonly IFolderViewModel DummyChild = new FolderViewModel();
        private bool mIsSelected;
        private bool mIsExpanded;

        private PathModel mModel;

        private readonly SortableObservableDictionaryCollection mFolders;
        private readonly IFolderViewModel _Parent;
        private string mVolumeLabel;

        private object mLockObject = new object();
        #endregion fields

        #region constructor
        /// <summary>
        /// Construct a folder viewmodel item from a path.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public FolderViewModel(PathModel model, IFolderViewModel parent)
            : this()
        {
            _Parent = parent;
            mFolders.AddItem(DummyChild);
            mModel = new PathModel(model);

            // Names of Logical drives cannot be changed with this
            if (mModel.PathType == FSItemType.LogicalDrive)
                this.IsReadOnly = true;
        }

        /// <summary>
        /// Construct a <seealso cref="FolderViewModel"/> item that represents a Windows
        /// file system folder object (eg.: FolderPath = 'C:\Temp\', FolderName = 'Temp').
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        protected FolderViewModel(string dir, IFolderViewModel parent)
           : this(new PathModel(dir, FSItemType.Folder), parent)
        {
        }

        /// <summary>
        /// Standard <seealso cref="FolderViewModel"/> constructor
        /// </summary>
        protected FolderViewModel()
          : base()
        {
            mIsExpanded = mIsSelected = false;

            mModel = null;

            // Add dummy folder by default to make tree view show expander by default
            mFolders = new SortableObservableDictionaryCollection();

            mVolumeLabel = null;
        }
        #endregion constructor

        #region properties
        /// <summary>
        /// Gets the name of this folder (without its root path component).
        /// </summary>
        public string FolderName
        {
            get
            {
                if (mModel != null)
                    return mModel.Name;

                return string.Empty;
            }
        }

        /// <summary>
        /// Get/set file system Path for this folder.
        /// </summary>
        public string FolderPath
        {
            get
            {
                if (mModel != null)
                    return mModel.Path;

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the parent object where this object is the child in the treeview.
        /// </summary>
        public IFolderViewModel Parent
        {
            get
            {
                return _Parent;
            }
        }

        /// <summary>
        /// Gets a folder item string for display purposes.
        /// This string can evaluete to 'C:\ (Windows)' for drives,
        /// if the 'C:\' drive was named 'Windows'.
        /// </summary>
        public string DisplayItemString
        {
            get
            {
                switch (ItemType)
                {
                    case FSItemType.LogicalDrive:
                        try
                        {
                            if (mVolumeLabel == null)
                            {
                                DriveInfo di = new System.IO.DriveInfo(FolderName);

                                if (di.IsReady == true)
                                    mVolumeLabel = di.VolumeLabel;
                                else
                                    return string.Format("{0} ({1})", FolderName, "device is not ready");
                            }

                            return string.Format("{0} {1}", FolderName, (string.IsNullOrEmpty(mVolumeLabel)
                                                                                ? string.Empty
                                                                                : string.Format("({0})", mVolumeLabel)));
                        }
                        catch (Exception exp)
                        {
                            ////base.ShowNotification("DriveInfo cannot be optained for:" + FolderName, FileSystemModels.Local.Strings.STR_MSG_UnknownError);

                            // Just return a folder name if everything else fails (drive may not be ready etc).
                            return string.Format("{0} ({1})", FolderName, exp.Message.Trim());
                        }

                    case FSItemType.Folder:
                    case FSItemType.Unknown:
                    default:
                        return FolderName;
                }
            }
        }

        /// <summary>
        /// Get/set observable collection of sub-folders of this folder.
        /// </summary>
        public IEnumerable<IFolderViewModel> Folders
        {
            get
            {
                return mFolders;
            }
        }

        /// <summary>
        /// Get/set whether this folder is currently selected or not.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return mIsSelected;
            }

            set
            {
                if (mIsSelected != value)
                {
                    Logger.Debug("Detail: set Folder IsSelected");

                    mIsSelected = value;

                    RaisePropertyChanged(() => IsSelected);

                    //// if (value == true)
                    ////    IsExpanded = true;                 // Default windows behaviour of expanding the selected folder
                }
            }
        }

        /// <summary>
        /// Get/set whether this folder is currently expanded or not.
        /// </summary>
        public bool IsExpanded
        {
            get
            {
                return mIsExpanded;
            }

            set
            {
                if (mIsExpanded != value)
                {
                    mIsExpanded = value;

                    RaisePropertyChanged(() => IsExpanded);
                }
            }
        }

        /// <summary>
        /// Gets the type of this item (eg: Folder, HardDisk etc...).
        /// </summary>
        public FSItemType ItemType
        {
            get
            {
                if (mModel != null)
                    return mModel.PathType;

                return FSItemType.Unknown;
            }
        }

        /// <summary>
        /// Determine whether child is a dummy (must be evaluated and replaced
        /// with real data) or not.
        /// </summary>
        public bool HasDummyChild
        {
            get
            {
                if (this.mFolders != null)
                {
                    if (this.mFolders.Count == 1)
                    {
                        if (this.mFolders[0] == DummyChild)
                            return true;
                    }
                }

                return false;
            }
        }
        #endregion properties

        #region methods
        /// <summary>
        /// Adds the folder item into the collection of sub-folders of this folder.
        /// </summary>
        /// <param name="item"></param>
        public void AddFolder(IFolderViewModel item)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                mFolders.AddItem(item);
            });
        }

        /// <summary>
        /// Rename the name of the folder into a new name.
        /// </summary>
        /// <param name="newFolderName"></param>
        public void RenameFolder(string newFolderName)
        {
            Logger.DebugFormat("Detail: Rename into new folder {0}:", newFolderName);

            lock (mLockObject)
            {
                try
                {
                    if (newFolderName != null)
                    {
                        PathModel sourceDir = new PathModel(FolderPath, FSItemType.Folder);
                        PathModel newFolderPath;

                        if (PathModel.RenameFileOrDirectory(sourceDir, newFolderName, out newFolderPath) == true)
                        {
                            ResetModel(newFolderPath);
                        }
                    }
                }
                catch (Exception exp)
                {
                    this.ShowNotification(FileSystemModels.Local.Strings.STR_RenameFolderErrorTitle, exp.Message);
                }
                finally
                {
                    RaisePropertyChanged(() => FolderName);
                    RaisePropertyChanged(() => FolderPath);
                    RaisePropertyChanged(() => DisplayItemString);
                }
            }
        }

        /// <summary>
        /// Attempts to find an item with the given name in the list of child items
        /// below this item and returns it or null.
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public IFolderViewModel TryGet(string folderName)
        {
            if (HasDummyChild == true)
                return null;

            return mFolders.TryGet(folderName);
        }

        /// <summary>
        /// Create a new folder with a standard name
        /// 'New folder n' underneath this folder.
        /// </summary>
        /// <returns>a viewmodel of the newly created directory or null</returns>
        public IFolderViewModel CreateNewDirectory()
        {
            Logger.DebugFormat("Detail: Create new directory with standard name.");

            lock (mLockObject)
            {
                try
                {
                    string defaultFolderName = FileSystemModels.Local.Strings.STR_NEW_DEFAULT_FOLDER_NAME;
                    var newSubFolder = PathModel.CreateDir(new PathModel(FolderPath, FSItemType.Folder), defaultFolderName);

                    if (newSubFolder != null)
                    {
                        var newFolder = new FolderViewModel(newSubFolder, this);
                        mFolders.AddItem(newFolder);

                        return newFolder;
                    }
                }
                catch (Exception exp)
                {
                    this.ShowNotification(FileSystemModels.Local.Strings.STR_MSG_UnknownError, exp.Message);
                }
            }

            return null;
        }

        internal static void AddFolder(FolderViewModel f, FolderViewModel item)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                f.AddFolder(item);
            });
        }

        /// <summary>
        /// Construct a <seealso cref="FolderViewModel"/> item that represents a Windows
        /// file system drive object (eg.: 'C:\').
        /// </summary>
        /// <param name="driveLetter"></param>
        /// <returns></returns>
        internal static FolderViewModel ConstructDriveFolderViewModel(string driveLetter)
        {
            return new FolderViewModel(new PathModel(driveLetter, FSItemType.LogicalDrive), null);
        }

        /// <summary>
        /// Load all sub-folders into the Folders collection.
        /// </summary>
        public void LoadFolders()
        {
            try
            {
                ClearFolders();

                string fullPath = Path.Combine(FolderPath, FolderName);

                if (FolderName.Contains(':'))                  // This is a drive
                    fullPath = string.Concat(FolderName, "\\");
                else
                    fullPath = FolderPath;

                foreach (string dir in Directory.GetDirectories(fullPath))
                    AddFolder(dir);
            }
            catch (UnauthorizedAccessException ae)
            {
                this.ShowNotification(FileSystemModels.Local.Strings.STR_MSG_UnknownError, ae.Message);
            }
            catch (IOException ie)
            {
                this.ShowNotification(FileSystemModels.Local.Strings.STR_MSG_UnknownError, ie.Message);
            }
        }

        /// <summary>
        /// Remove all sub-folders from a given folder.
        /// </summary>
        public void ClearFolders()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                mFolders.Clear();
            });
        }

        /// <summary>
        /// Add a new folder indicated by <paramref name="dir"/> as path
        /// into the sub-folder viewmodel collection of this folder item.
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        private FolderViewModel AddFolder(string dir)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(dir);

                // create the sub-structure only if this is not a hidden directory
                if ((di.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                {
                    var newFolder = new FolderViewModel(dir, this);

                    AddFolder(newFolder);

                    return newFolder;
                }
            }
            catch (UnauthorizedAccessException ae)
            {
                this.ShowNotification(FileSystemModels.Local.Strings.STR_MSG_UnknownError, ae.Message);
            }
            catch (Exception e)
            {
                this.ShowNotification(FileSystemModels.Local.Strings.STR_MSG_UnknownError, e.Message);
            }

            return null;
        }

        /// <summary>
        /// Method executes when item is renamed
        /// -> model name is required to be renamed and dependend
        /// properties are updated.
        /// </summary>
        /// <param name="model"></param>
        private void ResetModel(PathModel model)
        {
            var oldModel = mModel;

            mModel = new PathModel(model);

            if (oldModel == null && model == null)
                return;

            if (oldModel == null && model != null || oldModel != null && model == null)
            {
                RaisePropertyChanged(() => ItemType);
                RaisePropertyChanged(() => FolderName);
                RaisePropertyChanged(() => FolderPath);

                return;
            }

            if (oldModel.PathType != mModel.PathType)
                RaisePropertyChanged(() => ItemType);

            if (string.Compare(oldModel.Name, mModel.Name, true) != 0)
                RaisePropertyChanged(() => FolderName);

            if (string.Compare(oldModel.Path, mModel.Path, true) != 0)
                RaisePropertyChanged(() => FolderPath);
        }

        /// <summary>
        /// Shows a pop-notification message with the given title and text.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="imageIcon"></param>
        /// <returns>true if the event was succesfully fired.</returns>
        public new bool ShowNotification(string title, string message,
                                         BitmapImage imageIcon = null)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    return base.ShowNotification(title, message, imageIcon);
                });
            }
            catch
            {
            }

            return false;
        }
        #endregion methods
    }
}
