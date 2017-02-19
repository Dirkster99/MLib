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
        private static IContentDialogService _Instance = null;
        #endregion fields

        /// <summary>
        /// Gets an instance of the content dialog service component.
        /// This component displays content dialogs, including login,
        /// progress, and other special purpose dialogs ...
        /// 
        /// The instance is initialized with a <seealso cref="IMetroWindowService"/>
        /// in order to create external dialog windows without direct reference to
        /// MWindowLib which contains the actual functionality.
        /// </summary>
        public static IContentDialogService GetInstance(IMetroWindowService metroWindowService)
        {
            if (_Instance == null)
                _Instance = new Internal.ContentDialogServiceImpl(metroWindowService);

            return _Instance;
        }
    }
}
