namespace FileListViewTest.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Input;
    using FileListView.Command;
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
            this.FolderView = FileListViewTestFactory.Create();

            this.FolderView.AddRecentFolder(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            this.FolderView.AddRecentFolder(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), true);

            this.FolderView.AddFilter("Executeable files", "*.exe;*.bat");
            this.FolderView.AddFilter("Image files", "*.png;*.jpg;*.jpeg");
            this.FolderView.AddFilter("LaTex files", "*.tex");
            this.FolderView.AddFilter("Text files", "*.txt");
            this.FolderView.AddFilter("All Files", "*.*");
        }
        #endregion constructor

        #region properties
        /// <summary>
        /// Expose a viewmodel that controls the combobox folder drop down
        /// and the folder/file list view.
        /// </summary>
        public IControllerListViewModel FolderView { get; set; }

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
        /// Free resources (if any) when application exits.
        /// </summary>
        internal void ApplicationClosed()
        {

        }

        private void AddRecentFolder_Executed(object p)
        {
            string path;
            IControllerListViewModel vm;
            
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
            IControllerListViewModel vm;

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
                                          out string path, out IControllerListViewModel vm)
        {
            path = null;
            vm = null;

            if (l == null)
                return;

            foreach (var item in l)
            {
                if (item is IFSItemViewModel)
                {
                    var pathItem = item as IFSItemViewModel;

                    if (pathItem != null)
                        path = pathItem.FullPath;
                }
                else
                    if (item is IControllerListViewModel)
                    {
                        var vmItem = item as IControllerListViewModel;

                        if (vmItem != null)
                            vm = item as IControllerListViewModel;
                    }
            }

            if (path == null)
                path = @"C:\";
        }
        #endregion methods
    }
}
