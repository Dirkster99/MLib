namespace FolderControlsLib.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Windows.Input;
    using FolderControlsLib.Interfaces;
    using FolderControlsLib.ViewModels.Base;
    using FileSystemModels;
    using FileSystemModels.Events;
    using FileSystemModels.Models.FSItems.Base;
    using FileSystemModels.Interfaces;
    using System.Linq;

    /// <summary>
    /// Class implements a viewmodel that can be used for a
    /// combobox that can be used to browse to different folder locations.
    /// </summary>
    internal class FolderComboBoxViewModel : Base.ViewModelBase, IFolderComboBoxViewModel
    {
        #region fields
        private readonly ObservableCollection<IFolderItemViewModel> _CurrentItems;

        private IFolderItemViewModel _SelectedItem = null;

        private ICommand _SelectionChanged = null;
        private string _SelectedRecentLocation = string.Empty;

        private object _LockObject = new object();
        #endregion fields

        #region constructor
        /// <summary>
        /// Class constructor
        /// </summary>
        public FolderComboBoxViewModel()
        {
            this._CurrentItems = new ObservableCollection<IFolderItemViewModel>();
        }
        #endregion constructor

        #region Events
        /// <summary>
        /// Event is fired when the path in the text portion of the combobox is changed.
        /// </summary>
        public event EventHandler<FolderChangedEventArgs> RequestChangeOfDirectory;
        #endregion

        #region properties
        /// <summary>
        /// Expose a collection of file system items (folders and hard disks and ...) that
        /// can be selected and navigated to in this viewmodel.
        /// </summary>
        public IEnumerable<IFolderItemViewModel> CurrentItems
        {
            get
            {
                return this._CurrentItems;
            }
        }

        /// <summary>
        /// Gets/sets the currently selected file system viewmodel item.
        /// </summary>
        public IFolderItemViewModel SelectedItem
        {
            get
            {
                return _SelectedItem;
            }

            protected set
            {
                if (_SelectedItem != value)
                {
                    System.Console.WriteLine("SelectedItem changed to '{0}' -> '{1}'", _SelectedItem, value);
                    _SelectedItem = value;
                    RaisePropertyChanged(() => SelectedItem);

                    RaisePropertyChanged(() => CurrentFolder);
                    RaisePropertyChanged(() => CurrentFolderToolTip);
                }
            }
        }

        /// <summary>
        /// Get/sets viewmodel data pointing at the path
        /// of the currently selected folder.
        /// </summary>
        public string CurrentFolder
        {
            get
            {
                if (_SelectedItem != null)
                    return _SelectedItem.FullPath;

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets a string that can be displayed as a tooltip for the
        /// viewmodel data pointing at the path of the currently selected folder.
        /// </summary>
        public string CurrentFolderToolTip
        {
            get
            {
                if (string.IsNullOrEmpty(this.CurrentFolder) == false)
                    return string.Format("{0}\n{1}", this.CurrentFolder,
                                                     FileSystemModels.Local.Strings.SelectLocationCommand_TT);
                else
                    return FileSystemModels.Local.Strings.SelectLocationCommand_TT;
            }
        }

        #region commands
        /// <summary>
        /// Gets a command that should be invoked when the combobox view tells
        /// the viewmodel that the current path selection has changed
        /// (via selection changed event or keyup events).
        /// 
        /// The parameter p can be an array of objects
        /// containing objects of the FSItemVM type or
        /// p can also be string.
        /// 
        /// Each parameter item that adheres to the above types results in
        /// a OnCurrentPathChanged event being fired with the folder path
        /// as parameter.
        /// </summary>
        public ICommand SelectionChanged
        {
            get
            {
                if (_SelectionChanged == null)
                    _SelectionChanged = new RelayCommand<object>((p) => this.SelectionChanged_Executed(p));

                return _SelectionChanged;
            }
        }
        #endregion commands
        #endregion properties

        #region methods
        /// <summary>
        /// Can be invoked to refresh the currently visible set of data.
        /// </summary>
        public void PopulateView(IPathModel newPath)
        {
            lock (this._LockObject)
            {
                _CurrentItems.Clear();

                // add drives
                string pathroot = string.Empty;

                if (newPath == null)
                {
                    if (string.IsNullOrEmpty(this.CurrentFolder) == false)
                    {
                        try
                        {
                            pathroot = System.IO.Path.GetPathRoot(CurrentFolder);
                        }
                        catch
                        {
                            pathroot = string.Empty;
                        }
                    }
                }
                else
                    pathroot = System.IO.Path.GetPathRoot(newPath.Path);

                foreach (string s in Directory.GetLogicalDrives())
                {
                    IFolderItemViewModel info = FolderControlsLib.Factory.CreateLogicalDrive(s);
                    this._CurrentItems.Add(info);

                    // add items under current folder if we currently create the root folder of the current path
                    if (string.IsNullOrEmpty(pathroot) == false && string.Compare(pathroot, s, true) == 0)
                    {
                        string[] dirs;

                        if (newPath == null)
                            dirs = PathFactory.GetDirectories(CurrentFolder);
                        else
                            dirs = PathFactory.GetDirectories(newPath.Path);

                        for (int i = 1; i < dirs.Length; i++)
                        {
                            string curdir = PathFactory.Join(dirs, 0, i + 1);

                            var curPath = PathFactory.Create(curdir, FSItemType.Folder);
                            info = new FolderItemViewModel(curPath, dirs[i], false, i * 10);

                            this._CurrentItems.Add(info);
                        }

                        SelectedItem = _CurrentItems.Last();
                    }
                }

                // Force a selection on to the control when there is no selected item, yet
                if (this._CurrentItems != null && SelectedItem == null)
                {
                    if (this._CurrentItems.Count > 0)
                        this.SelectedItem = this._CurrentItems.Last();
                }
            }
        }

        private void InternalPopulateView(IPathModel newPath
                                        , bool sendNotification)
        {
            PopulateView(newPath);

            if (sendNotification == true && this.SelectedItem != null)
            {
                if (this.RequestChangeOfDirectory != null)
                    this.RequestChangeOfDirectory(this, new FolderChangedEventArgs(this.SelectedItem.GetModel));
            }
        }

        /// <summary>
        /// Method executes when the SelectionChanged command is invoked.
        /// The parameter <paramref name="p"/> can be an array of objects
        /// containing objects of the <seealso cref="IFolderItemViewModel"/> type
        /// or p can also be string.
        /// 
        /// Each parameter item that adheres to the above types results in
        /// a OnCurrentPathChanged event being fired with the folder path
        /// as parameter.
        /// </summary>
        /// <param name="p"></param>
        private void SelectionChanged_Executed(object p)
        {
            if (p == null)
                return;

            // Check if the given parameter is a string, fire a corresponding event if so...
            if (p is string)
            {
                IPathModel param = null;
                try
                {
                    param = PathFactory.Create(p as string);
                }
                catch
                {
                    return;   // Control will refuse to select an unknown/non-existing item
                }

                // This breaks a possible recursion, if a new view is requested even though its
                // already available, because this could, otherwise, change the SelectedItem
                // which in turn could request another PopulateView(...) -> SelectedItem etc ...
                if (SelectedItem != null)
                {
                    if (SelectedItem.Equals(param))
                        return;
                }

                InternalPopulateView(PathFactory.Create(p as string, FSItemType.Folder), true);
            }
            else
            {
                if (p is object[])
                {
                    var param = p as object[];

                    if (param != null)
                    {
                        if (param.Length > 0)
                        {
                            var newPath = param[param.Length - 1] as IFolderItemViewModel;

                            if (newPath != null)
                            {
                                var model = newPath.GetModel;

                                // This breaks a possible recursion, if a new view is requested even though its
                                // already available, because this could, otherwise, change the SelectedItem
                                // which in turn could request another PopulateView(...) -> SelectedItem etc ...
                                if (model.Equals(SelectedItem.GetModel))
                                    return;

                                InternalPopulateView(model, true);
                            }
                        }
                    }
                }
            }
        }
        #endregion methods
    }
}
