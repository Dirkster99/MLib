namespace FileSystemModels.Models.FSItems
{
    using FileSystemModels.Interfaces;
    using System.IO;
    using System.Security;

    public class FileModel : Base.FileSystemModel
    {
        #region fields
        private readonly FileInfo _File;
        #endregion fields

        #region constructors
        /// <summary>
        /// Parameterized class  constructor
        /// </summary>
        /// <param name="model"></param>
        [SecuritySafeCritical]
        public FileModel(IPathModel model)
          : base(model)
        {
            _File = new FileInfo(model.Path);
        }
        #endregion constructors

        #region properties
        public DirectoryInfo Directory
        {
            get
            {
                return _File.Directory;
            }
        }

        public string DirectoryName
        {
            get
            {
                return _File.DirectoryName;
            }
        }

        public bool Exists
        {
            get
            {
                return _File.Exists;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return _File.IsReadOnly;
            }
        }

        public long Length
        {
            get
            {
                return _File.Length;
            }
        }
        #endregion properties
    }
}
