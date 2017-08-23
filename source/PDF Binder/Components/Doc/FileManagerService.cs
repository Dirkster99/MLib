using Doc.DocManager.Interfaces;

namespace Doc
{
    public class FileManagerService
    {
        #region properties
        /// <summary>
        /// Gets an instance of the MessageBox service component.
        /// This component displays message boxes, including stack traces,
        /// in an integrated WPF themed manner..
        /// </summary>
        public static IFileManager Instance
        {
            get
            {
                return new DocManager.FileManagerServiceImpl();
            }
        }
        #endregion properties
    }
}
