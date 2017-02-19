namespace MLib
{
    /// <summary>
    /// The AppearanceManager class manages all WPF theme related items.
    /// Its location in the hierarchy is between the viewmodels and the
    /// themes settings service.
    /// </summary>
    public class AppearanceManager
    {
        #region fields
        private static readonly IAppearanceManager _instance = null;
        #endregion fields

        #region constructor
        static AppearanceManager()
        {
            _instance = new Internal.AppearanceManagerImp();
        }
        #endregion constructor

        #region properties
        /// <summary>
        /// Gets an instance of the Appearance Manager component.
        /// This component manages theme related things, such as:
        /// theme selection Dark/Light etc..
        /// </summary>
        public static IAppearanceManager Instance
        {
            get
            {
                return _instance;
            }
        }
        #endregion properties
    }
}
