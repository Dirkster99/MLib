namespace PDF_Binder.ViewModels
{
    using PDFBinderLib;

    /// <summary>
    /// The file viewmodel keeps track of a files name and path in the filesystem.
    /// </summary>
    public class FileViewModel : Base.ViewModelBase, IPDFFile
    {
        #region private members
        private string _Name;
        private string _Path;
        #endregion private members

        #region constructor
        /// <summary>
        /// Standard constructor
        /// </summary>
        public FileViewModel()
        {
            this._Path = string.Empty;
            this._Name = string.Empty;
        }

        /// <summary>
        /// Constructs a viewmodel from default parameters path and name of file.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="filename"></param>
        public FileViewModel(string path, string filename)
        {
            this._Path = (path == null ? string.Empty : path);
            this._Name = (filename == null ? string.Empty : filename);
        }

        /// <summary>
        /// Cosntruct a viewmodel from 1 path string.
        /// </summary>
        /// <param name="path_filename"></param>
        public FileViewModel(string path_filename)
        {
            SetPathAndFileName(path_filename);
        }
        #endregion constructor

        #region properties
        /// <summary>
        /// Gets/sets the path in which the file will be/is stored.
        /// </summary>
        public string Path
        {
            get { return _Path; }
            set
            {
                if (_Path != value)
                {
                    _Path = value;
                    this.RaisePropertyChanged(() => this.Path);
                }
            }
        }

        /// <summary>
        /// Gets/sets the file name with which the file will be/is stored.
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    this.RaisePropertyChanged(() => this.Name);
                }
            }
        }

        /// <summary>
        /// Gets the file name and path with which the file will be/is stored.
        /// </summary>
        public string FileName
        {
            get
            {
                try
                {
                    return System.IO.Path.Combine(_Path, _Name);
                }
                catch
                {
                }

                return string.Empty;
            }

            set
            {
                SetPathAndFileName(value);
            }
        }

        /// <summary>
        /// Standard method useful for debugging etc.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Path + @"\" + this.Name;
        }
        #endregion properties

        #region methodes
        /// <summary>
        /// Set path and file name from one path string.
        /// </summary>
        /// <param name="value"></param>
        protected void SetPathAndFileName(string value)
        {
            try
            {
                Path = System.IO.Path.GetDirectoryName(value);
            }
            catch
            {
                Path = string.Empty;
            }

            try
            {
                Name = System.IO.Path.GetFileName(value);
            }
            catch
            {
                Name = string.Empty;
            }
        }
        #endregion methodes
    }
}
