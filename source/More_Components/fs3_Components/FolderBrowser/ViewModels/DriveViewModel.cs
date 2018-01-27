namespace FolderBrowser.ViewModels
{
    using FileSystemModels.Models;
    using FileSystemModels.Models.FSItems;
    using FolderBrowser.Interfaces;
    using System;

    internal class DriveViewModel : ItemViewModel, IDriveViewModel
    {
        #region fields
        private object _LockObject = new object();
        #endregion fields

        #region constructors
        /// <summary>
        /// Constructs a drive's viewmodel.
        /// </summary>
        public DriveViewModel(PathModel model, IItemViewModel parent)
           : base(model, parent)
        {
        }
        #endregion constructors

        #region methods
        /// <summary>
        /// Load all sub-folders into the Folders collection.
        /// </summary>
        public override void LoadFolders()
        {
            FolderViewModel.LoadFolders(this);
        }

        /// <summary>
        /// Create a new folder with a standard name
        /// 'New folder n' underneath this folder.
        /// </summary>
        /// <returns>a viewmodel of the newly created directory or null</returns>
        public override IItemViewModel CreateNewDirectory()
        {
            Logger.DebugFormat("Detail: Create new directory with standard name.");

            lock (_LockObject)
            {
                try
                {
                    string defaultFolderName = FileSystemModels.Local.Strings.STR_NEW_DEFAULT_FOLDER_NAME;
                    var model = new PathModel(ItemPath, FSItemType.Folder);
                    var newSubFolder = PathModel.CreateDir(model, defaultFolderName);

                    if (newSubFolder != null)
                    {
                        var item = new FolderViewModel(newSubFolder, this);
                        ChildAdd(item);
                        return item;
                    }
                }
                catch (Exception exp)
                {
                    this.ShowNotification(FileSystemModels.Local.Strings.STR_MSG_UnknownError, exp.Message);
                }
            }

            return null;
        }
        #endregion methods
    }
}
