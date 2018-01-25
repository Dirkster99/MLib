namespace FolderBrowser.ViewModels
{
    using FolderBrowser.Interfaces;
    using FsCore.Collections;
    using System.Collections.Generic;

    internal class SortableObservableDictionaryCollection : SortableObservableCollection<IFolderViewModel>
    {
        Dictionary<string, IFolderViewModel> _dictionary = null;

        public SortableObservableDictionaryCollection()
        {
            _dictionary = new Dictionary<string, IFolderViewModel>();
        }

        public bool AddItem(IFolderViewModel item)
        {
            if (string.IsNullOrEmpty(item.FolderName) == true)
                _dictionary.Add(string.Empty, item);
            else
                _dictionary.Add(item.FolderName.ToLower(), item);

            this.Add(item);

            return true;
        }

        public bool RemoveItem(IFolderViewModel item)
        {
            _dictionary.Remove(item.FolderName.ToLower());
            this.Remove(item);

            return true;
        }

        public IFolderViewModel TryGet(string key)
        {
            IFolderViewModel o;

            if (_dictionary.TryGetValue(key.ToLower(), out o))
                return o;

            return null;
        }

        public void RenameItem(IFolderViewModel item, string newName)
        {
            _dictionary.Remove(item.FolderName.ToLower());
            item.RenameFolder(newName);
            _dictionary.Add(newName.ToLower(), item);
        }

        public new void Clear()
        {
            _dictionary.Clear();
            base.Clear();
        }
    }
}
