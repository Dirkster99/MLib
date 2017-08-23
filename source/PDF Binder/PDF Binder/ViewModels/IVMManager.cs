namespace PDF_Binder.ViewModels
{
    using VMManagement;

    public interface IVMManager
    {
        /// <summary>
        /// Gets a viewmodel association based on an item's keys.
        /// </summary>
        /// <param name="itemKey"></param>
        /// <returns></returns>
        VMItem GetVMItem(int itemKey);

        /// <summary>
        /// Add a new viewmodel instance from its item data and instance reference.
        /// </summary>
        /// <param name="itemKey"></param>
        /// <param name="name"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        VMItem AddVMItem(int itemKey, string name, object instance = null);

        /// <summary>
        /// Gets a viewmodel association based on an item's keys and
        /// removes the retrieved item from the collection.
        /// </summary>
        /// <param name="itemKey"></param>
        /// <returns></returns>
        VMItem RemoveVMItem(int applicationViewModel);

        VMItem GetCurrentViewModel();

        VMItem SetCurrentViewModel(int itemKey);

        /// <summary>
        /// Removes the current viewmodel from the current viewmodel property.
        /// Call this when application shuts down to make sure all viewmodels
        /// can be destroyed without being currently active.
        /// </summary>
        void UnsetCurrentViewModel();
    }
}
