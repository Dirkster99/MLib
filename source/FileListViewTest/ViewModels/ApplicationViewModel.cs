namespace FileListViewTest.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Input;
    using FileListViewTest.Command;
    using FileListViewTest.Interfaces;
    using FileSystemModels.Interfaces;
    using FolderBrowser;

    /// <summary>
    /// Class implements an application viewmodel that manages the test application.
    /// </summary>
    public class ApplicationViewModel : Base.ViewModelBase
    {
        #region fields
        private ICommand mAddRecentFolder;
        private ICommand mRemoveRecentFolder;
        #endregion fields

        #region constructor
        /// <summary>
        /// Class constructor
        /// </summary>
        public ApplicationViewModel()
        {
            FolderView = FileListViewTestFactory.CreateList();
            FolderTreeView = FileListViewTestFactory.CreateTreeList();

            FolderView.AddRecentFolder(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            FolderView.AddRecentFolder(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), true);

            FolderView.AddFilter("Executeable files", "*.exe;*.bat");
            FolderView.AddFilter("Image files", "*.png;*.jpg;*.jpeg");
            FolderView.AddFilter("LaTex files", "*.tex");
            FolderView.AddFilter("Text files", "*.txt");
            FolderView.AddFilter("All Files", "*.*");
        }
        #endregion constructor

        #region properties
        /// <summary>
        /// Expose a viewmodel that controls the combobox folder drop down
        /// and the folder/file list view.
        /// </summary>
        public IListControllerViewModel FolderView { get; }

        public ITreeListControllerViewModel FolderTreeView { get; }

        #region Commands for test case without folderBrowser
        /// <summary>
        /// Add a folder to the list of recent folders.
        /// </summary>
        public ICommand AddRecentFolder
        {
          get
          {
            if (this.mAddRecentFolder == null)
              this.mAddRecentFolder = new RelayCommand<object>((p) =>
              {
                this.AddRecentFolder_Executed(p);
              });
        
            return this.mAddRecentFolder;
          }
        }
        
        /// <summary>
        /// Remove a folder from the list of recent folders.
        /// </summary>
        public ICommand RemoveRecentFolder
        {
          get
          {
            if (this.mRemoveRecentFolder == null)
              this.mRemoveRecentFolder = new RelayCommand<object>(
                   (p) => this.RemoveRecentFolder_Executed(p),
                   (p) => this.FolderView.SelectedRecentLocation != null);
        
            return this.mRemoveRecentFolder;
          }
        }
        #endregion Commands for test case without folderBrowser
        #endregion properties

        #region methods
        /// <summary>
        /// Call this method to initialize viewmodel items that might need to display
        /// progress information (e.g. call this in OnLoad() method of view)
        /// </summary>
        /// <param name="path"></param>
        internal void InitializeViewModel(IPathModel path)
        {
            FolderView.NavigateToFolder(path);
            FolderTreeView.NavigateToFolder(path);
        }

        /// <summary>
        /// Free resources (if any) when application exits.
        /// </summary>
        internal void ApplicationClosed()
        {

        }

        private void AddRecentFolder_Executed(object p)
        {
            string path;
            IListControllerViewModel vm;
            
            this.ResolveParameterList(p as List<object>, out path, out vm);
            
            if (vm == null)
              return;

            var browser = FolderBrowserFactory.CreateBrowserViewModel();

            path = (string.IsNullOrEmpty(path) == true ? @"C:\" : path);
            browser.InitialPath = path;

            var dlg = new FolderBrowser.Views.FolderBrowserDialog();
            
            var dlgViewModel = FolderBrowserFactory.CreateDialogViewModel(
                browser, vm.RecentFolders.CloneBookmark());
            
            dlg.DataContext = dlgViewModel;
            
            bool? bResult = dlg.ShowDialog();
            
            if (dlgViewModel.DialogCloseResult == true || bResult == true)
            {
                vm.CloneBookmarks(dlgViewModel.BookmarkedLocations, vm.RecentFolders);
                vm.AddRecentFolder(dlgViewModel.TreeBrowser.SelectedFolder, true);
            }
        }

        private void RemoveRecentFolder_Executed(object p)
        {
            string path;
            IListControllerViewModel vm;

            this.ResolveParameterList(p as List<object>, out path, out vm);

            if (vm == null || path == null)
                return;

            vm.RemoveRecentFolder(path);
        }

        /// <summary>
        /// Resolves the parameterlist retrieved from a multibinding command parameter
        /// which has packed parameters via List<object> container into 1 object.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="path"></param>
        /// <param name="vm"></param>
        private void ResolveParameterList(List<object> l,
                                          out string path, out IListControllerViewModel vm)
        {
            path = null;
            vm = null;

            if (l == null)
                return;

            foreach (var item in l)
            {
                if (item is IListItemViewModel)
                {
                    var pathItem = item as IListItemViewModel;

                    if (pathItem != null)
                        path = pathItem.FullPath;
                }
                else
                    if (item is IListControllerViewModel)
                    {
                        var vmItem = item as IListControllerViewModel;

                        if (vmItem != null)
                            vm = item as IListControllerViewModel;
                    }
            }

            if (path == null)
                path = @"C:\";
        }
        #endregion methods
    }
}
