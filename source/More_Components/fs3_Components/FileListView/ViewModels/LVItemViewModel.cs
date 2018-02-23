namespace FileListView.ViewModels
{
    using System;
    using System.IO;
    using FileListView.Interfaces;
    using FileSystemModels;
    using FileSystemModels.Interfaces;
    using FileSystemModels.Models.FSItems.Base;
    using InplaceEditBoxLib.ViewModels;

    /// <summary>
    /// The Viewmodel for file system items
    /// </summary>
    internal class LVItemViewModel : EditInPlaceViewModel, ILVItemViewModel
    {
        #region fields
        /// <summary>
        /// Logger facility
        /// </summary>
        protected new static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string _DisplayName;
        private IPathModel _PathObject;
        private string _VolumeLabel;
        #endregion fields

        #region constructor
        /// <summary>
        /// class constructor
        /// </summary>
        /// <param name="curdir"></param>
        /// <param name="displayName"></param>
        /// <param name="itemType"></param>
        /// <param name="showIcon"></param>
        /// <param name="indentation"></param>
        public LVItemViewModel(string curdir,
                        FSItemType itemType,
                        string displayName,
                        bool showIcon)
          : this(curdir, itemType, displayName)
        {
            this.ShowIcon = showIcon;
        }

        /// <summary>
        /// Sets the display name of this item.
        /// </summary>
        /// <param name="stringToDisplay"></param>
        internal void SetDisplayName(string stringToDisplay)
        {
            DisplayName = stringToDisplay;
        }

        /// <summary>
        /// class constructor
        /// </summary>
        /// <param name="curdir"></param>
        /// <param name="itemType"></param>
        /// <param name="displayName"></param>
        /// <param name="indentation"></param>
        public LVItemViewModel(string curdir,
                        FSItemType itemType,
                        string displayName,
                        int indentation = 0)
          : this()
        {
            this._PathObject = PathFactory.Create(curdir, itemType);
            this.DisplayName = displayName;
        }

        /// <summary>
        /// class constructor
        /// </summary>
        /// <param name="model"></param>
        /// <param name="displayName"></param>
        /// <param name="indentation"></param>
        public LVItemViewModel(IPathModel model,
                        string displayName,
                        bool isReadOnly = false)
            : this()
        {
            _PathObject = model.Clone() as IPathModel;
            DisplayName = displayName;
            IsReadOnly = isReadOnly;
        }

        /// <summary>
        /// Hidden standard class constructor
        /// </summary>
        protected LVItemViewModel()
        {
            _PathObject = null;
            _VolumeLabel = null;
            ShowIcon = true;
        }
        #endregion constructor

        #region properties
        /// <summary>
        /// Gets the type (folder, file) of this item
        /// </summary>
        public FSItemType ItemType
        {
            get
            {
                return (this._PathObject != null ? this._PathObject.PathType : FSItemType.Unknown);
            }
        }

        /// <summary>
        /// Gets the path to this item
        /// </summary>
        public string ItemPath
        {
            get
            {
                return (this._PathObject != null ? this._PathObject.Path : null);
            }
        }

        /// <summary>
        /// Gets whether this folder is currently expanded or not.
        /// 
        /// This viewmodel, currently, has no use case for an expanded item.
        /// Therefore, this property returns a constanst false value.
        /// </summary>
        public bool IsExpanded { get { return false; } }

        /// <summary>
        /// Gets a name that can be used for display
        /// (is not necessarily the same as path)
        /// </summary>
        public string DisplayName
        {
            get
            {
                return this._DisplayName;
            }

            protected set
            {
                if (this._DisplayName != value)
                {
                    this._DisplayName = value;
                    this.RaisePropertyChanged(() => this.DisplayName);
                }
            }
        }

        /// <summary>
        /// Gets a copy of the internal <seealso cref="PathModel"/> object.
        /// </summary>
        public IPathModel GetModel
        {
            get
            {
                return this._PathObject.Clone() as IPathModel;
            }
        }

        /// <summary>
        /// Gets whether or not to show an Icon for this item.
        /// </summary>
        public bool ShowIcon { get; private set; }
        #endregion properties

        #region methods
        /// <summary>
        /// Standard method to display contents of this class.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.ItemPath;
        }

        /// <summary>
        /// Determine whether a given path is an exeisting directory or not.
        /// </summary>
        /// <returns>true if this directory exists and otherwise false</returns>
        public bool DirectoryPathExists()
        {
            return this._PathObject.DirectoryPathExists();
        }

        /// <summary>
        /// Gets a folder item string for display purposes.
        /// This string can evaluete to 'C:\ (Windows)' for drives,
        /// if the 'C:\' drive was named 'Windows'.
        /// </summary>
        public string DisplayItemString()
        {
            switch (this._PathObject.PathType)
            {
                case FSItemType.LogicalDrive:
                    try
                    {
                        if (this._VolumeLabel == null)
                        {
                            DriveInfo di = new System.IO.DriveInfo(this.ItemPath);

                            if (di.IsReady == true)
                                this._VolumeLabel = di.VolumeLabel;
                            else
                                return string.Format("{0} ({1})", this.ItemPath, FileSystemModels.Local.Strings.STR_MSG_DEVICE_NOT_READY);
                        }

                        return string.Format("{0} {1}", this.ItemPath, (string.IsNullOrEmpty(this._VolumeLabel)
                                                                        ? string.Empty
                                                                        : string.Format("({0})", this._VolumeLabel)));
                    }
                    catch (Exception exp)
                    {
                        Logger.Warn("DriveInfo cannot be optained for:" + this.ItemPath, exp);

                        // Just return a folder name if everything else fails (drive may not be ready etc).
                        return string.Format("{0} ({1})", this.ItemPath, exp.Message.Trim());
                    }

                case FSItemType.Folder:
                case FSItemType.File:
                case FSItemType.Unknown:
                default:
                    return this.ItemPath;
            }
        }

        /// <summary>
        /// Rename the name of a folder or file into a new name.
        /// 
        /// This includes renaming the item in the file system.
        /// </summary>
        /// <param name="newFolderName"></param>
        public void RenameFileOrFolder(string newFolderName)
        {
            try
            {
                if (newFolderName != null)
                {
                    IPathModel newFolderPath;

                    if (PathFactory.RenameFileOrDirectory(this._PathObject, newFolderName, out newFolderPath) == true)
                    {
                        this._PathObject = newFolderPath;
                        this.DisplayName = newFolderPath.Name;
                    }
                }
            }
            catch (Exception exp)
            {
                Logger.Error(string.Format("Rename into '{0}' was not succesful.", newFolderName), exp);

                base.ShowNotification(FileSystemModels.Local.Strings.STR_RenameFolderErrorTitle, exp.Message);
            }
            finally
            {
                this.RaisePropertyChanged(() => this.ItemPath);
            }
        }
        #endregion methods
    }
}
