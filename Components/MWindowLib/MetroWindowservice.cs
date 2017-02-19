namespace MWindowLib
{
    using MWindowInterfacesLib.Interfaces;

    /// <summary>
    /// Retrieves a reference to an <seealso cref="IMetroWindowService"/> component.
    /// </summary>
    public class MetroWindowService
    {
        #region fields
        private static readonly IMetroWindowService _Instance;
        #endregion fields

        #region costructors
        static MetroWindowService()
        {
            _Instance = new Internal.MetroWindowServiceImpl();
        }
        #endregion costructors

        #region properties
        /// <summary>
        /// Gets an instance of the MetroWindowService service component.
        /// This component contains utility functions that can be used to
        /// create metro windows in the context of other services...
        /// </summary>
        public static IMetroWindowService Instance
        {
            get
            {
                return _Instance;
            }
        }
        #endregion properties
    }
}
