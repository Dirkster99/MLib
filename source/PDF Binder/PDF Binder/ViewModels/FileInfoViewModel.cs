namespace PDF_Binder.ViewModels
{
    using PDFBinderLib;

    public class FileInfoViewModel : FileViewModel, IPDFStateFile
    {
        #region fields
        PDFTestResult _State;
        #endregion fields

        #region constructor
        /// <summary>
        /// Standard constructor
        /// </summary>
        public FileInfoViewModel()
            : base()
        {
            _State = PDFTestResult.Unknown;
        }

        /// <summary>
        /// Constructs a viewmodel from default parameters path and name of file.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="filename"></param>
        public FileInfoViewModel(string path, string filename)
            : base(path, filename)
        {
            _State = PDFTestResult.Unknown;
        }

        /// <summary>
        /// Cosntruct a viewmodel from 1 path string.
        /// </summary>
        /// <param name="path_filename"></param>
        public FileInfoViewModel(string path_filename)
            : base(path_filename)
        {
            _State = PDFTestResult.Unknown;
        }
        #endregion

        #region properties
        /// <summary>
        /// Gets a state of a file which can be one of the states
        /// indicated by the <seealso cref="PDFTestResult"/> enumeration.
        /// </summary>
        public PDFTestResult State
        {
            get { return _State; }
            set
            {
                if (_State != value)
                {
                    _State = value;
                    this.RaisePropertyChanged(() => this.State);
                }
            }
        }
        #endregion properties

        #region methodes
        #endregion methodes
    }
}
