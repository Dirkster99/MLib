namespace HistoryControlLib
{
    using HistoryControlLib.Interfaces;
    using HistoryControlLib.ViewModels;

    /// <summary>
    /// Implements a generic factory for creating browse history
    /// objects that adhere to the <seealso cref="IBrowseHistory{T}"/>
    /// interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class Factory<T>
    {
        /// <summary>
        /// Hidden class constructor.
        /// </summary>
        private Factory()
        {
        }

        /// <summary>
        /// Creates a browse history object that keeps track of a users browse
        /// hostory based on the items defined through {T}.
        /// </summary>
        /// <returns></returns>
        public static IBrowseHistory<T> CreateBrowseNavigator()
        {
            return new BrowseHistory<T>();
        }
    }
}
