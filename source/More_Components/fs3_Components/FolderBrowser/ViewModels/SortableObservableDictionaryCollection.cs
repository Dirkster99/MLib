namespace FolderBrowser.ViewModels
{
    using FolderBrowser.Interfaces;
    using FsCore.Collections;
    using System.Collections.Generic;

    internal class SortableObservableDictionaryCollection : SortableObservableCollection<IItemViewModel>
    {
        Dictionary<string, IItemViewModel> _dictionary = null;

        public SortableObservableDictionaryCollection()
        {
            _dictionary = new Dictionary<string, IItemViewModel>();
        }

        public bool AddItem(IItemViewModel item)
        {
            if (string.IsNullOrEmpty(item.ItemName) == true)
                _dictionary.Add(string.Empty, item);
            else
                _dictionary.Add(item.ItemName.ToLower(), item);

            this.Add(item);

            return true;
        }

        public bool RemoveItem(IItemViewModel item)
        {
            _dictionary.Remove(item.ItemName.ToLower());
            this.Remove(item);

            return true;
        }

        public IItemViewModel TryGet(string key)
        {
            IItemViewModel o;

            if (_dictionary.TryGetValue(key.ToLower(), out o))
                return o;

            return null;
        }

        public void RenameItem(IItemViewModel item, string newName)
        {
            _dictionary.Remove(item.ItemName.ToLower());
            item.Rename(newName);
            _dictionary.Add(newName.ToLower(), item);
        }

        public new void Clear()
        {
            _dictionary.Clear();
            base.Clear();
        }
    }
}
