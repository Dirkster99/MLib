namespace TreeViewDemo.Demos.Models.FSItems.Base
{
    using FileSystemModels.Interfaces;
    using FileSystemModels.Models.FSItems.Base;
    using System.Diagnostics;

    /// <summary>
    /// This class models the common aspects of all classes that model
    /// file system items (drive, folder, files) and their capabilities.
    /// </summary>
    public abstract class FileSystemModel
    {
        #region fields
        private readonly IPathModel mModel;
        #endregion fields

        #region constructors
        /// <summary>
        /// Parameterized class constructor
        /// </summary>
        public FileSystemModel(IPathModel model)
        {
            mModel = model;
            Debug.Assert(model != null, "Construction of FSItem without PathModel is not supported!");
        }

        /// <summary>
        /// Hidden class constructor
        /// </summary>
        protected FileSystemModel()
        {
            mModel = null;
        }
        #endregion constructors

        #region properties
        /// <summary>
        /// Gets the path model for this filesystem item.
        /// </summary>
        public IPathModel Model
        {
            get
            {
                return mModel;
            }
        }

        /// <summary>
        /// Gets the type of file system item (Drive, folder, file)
        /// represented by this object.
        /// </summary>
        public FSItemType ItemType
        {
            get
            {
                return mModel.PathType;
            }
        }

        /// <summary>
        /// Gets the noe of the drive, file, or folder
        /// represented by this file system item.
        /// </summary>
        public string Name
        {
            get
            {
                return mModel.Name;
            }
        }
        #endregion properties
    }
}
