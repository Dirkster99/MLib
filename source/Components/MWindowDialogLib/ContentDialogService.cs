namespace MWindowDialogLib
{
    using MWindowInterfacesLib.Interfaces;

    /// <summary>
    /// This service is the root item for all other content dialog
    /// related services in this assembly.
    /// </summary>
    public class ContentDialogService
    {
        #region fields
        private static readonly IContentDialogService _Instance;
        #endregion fields

        #region costructors
        static ContentDialogService()
        {
            _Instance = new Internal.ContentDialogServiceImpl();
        }
        #endregion costructors

        #region properties
        /// <summary>
        /// Gets an instance of the content dialog service component.
        /// This component displays content dialogs, including login,
        /// progress, and other special purpose dialogs ...
        /// 
        /// The instance is initialized with a <seealso cref="IMetroWindowService"/>
        /// in order to create external dialog windows if and when a main window
        /// is not available, because the appliction MainWindow:
        /// 1) Is not yet created on start-up of appliaction or
        /// 2) already gone on shut-down or crash of application.
        /// </summary>
        public static IContentDialogService Instance
        {
            get
            {
                return _Instance;
            }
        }
        #endregion properties
    }
}
