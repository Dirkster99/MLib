namespace PDF_Binder.ViewModels.VMManagement
{
    using System.Collections.Generic;

    public class VMManager : IVMManager
    {
        #region fields
        private Dictionary<int, VMItem> _vmItems = new Dictionary<int, VMItem>();

        private VMItem _CurrentViewModel = null;
        #endregion fields

        #region constructors
        public VMManager(VMItem[] items)
        {
            if (_vmItems.Count > 0)
                _vmItems.Clear();

            if (items != null)
            {
                foreach (var item in items)
                {
                    _vmItems.Add(item.ItemKey, new VMItem(item));
                }
            }
        }

        protected VMManager()
        {
            _vmItems = new Dictionary<int, VMItem>();
        }
        #endregion constructors

        #region methods
        /// <summary>
        /// Gets a viewmodel association based on an item's keys.
        /// </summary>
        /// <param name="itemKey"></param>
        /// <returns></returns>
        public VMItem GetVMItem(int itemKey)
        {
            VMItem o;
            _vmItems.TryGetValue(itemKey, out o);

            return o;
        }

        /// <summary>
        /// Add a new viewmodel instance from its item data and instance reference.
        /// </summary>
        /// <param name="itemKey"></param>
        /// <param name="name"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        public VMItem AddVMItem(int itemKey, string name, object instance = null)
        {
            var newItem = new VMItem(itemKey, name, instance);

            _vmItems.Add(itemKey, newItem);

            return newItem;
        }

        /// <summary>
        /// Gets a viewmodel association based on an item's keys and
        /// removes the retrieved item from the collection.
        /// </summary>
        /// <param name="itemKey"></param>
        /// <returns></returns>
        public VMItem RemoveVMItem(int itemKey)
        {
            var item = GetVMItem(itemKey);

            if (item != null)
                _vmItems.Remove(itemKey);

            if(_CurrentViewModel == item)
                throw new System.Exception("Current ViewModel cannot be removed from collection.");

            return item;
        }

        public VMItem GetCurrentViewModel()
        {
            if (_CurrentViewModel == null)
                throw new System.Exception("Current ViewModel cannot be null on retrieval.");

            return _CurrentViewModel;
        }

        public VMItem SetCurrentViewModel(int itemKey)
        {
            var item = GetVMItem(itemKey);

            if (item == null)
                throw new System.Exception("Current ViewModel cannot be set since its not added in collection.");

            _CurrentViewModel = item;

            return _CurrentViewModel;
        }

        /// <summary>
        /// Removes the current viewmodel from the current viewmodel property.
        /// Call this when application shuts down to make sure all viewmodels
        /// can be destroyed without being currently active.
        /// </summary>
        public void UnsetCurrentViewModel()
        {
            _CurrentViewModel = null;
        }
        #endregion methods
    }
}
