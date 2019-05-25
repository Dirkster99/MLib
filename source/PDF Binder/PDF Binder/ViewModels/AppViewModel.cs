namespace PDF_Binder.ViewModels
{
    using Base;
    using Doc.DocManager.Interfaces;
    using ExplorerLib;
    using FileSystemModels;
    using FileSystemModels.Browse;
    using FileSystemModels.Interfaces.Bookmark;
    using FolderBrowser;
    using MLib.Interfaces;
    using MLib.Themes;
    using MWindowInterfacesLib.Interfaces;
    using MWindowInterfacesLib.MsgBox.Enums;
    using PDF_Binder.ViewModels.FBContentDialog;
    using PDF_Binder.ViewModels.VMManagement;
    using PDFBinderLib;
    using PDFBinderLib.Implementations;
    using Settings.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Main ViewModel vlass that manages session start-up, life span, and shutdown
    /// of the application.
    /// </summary>
    public class AppViewModel : Base.ViewModelBase, IDisposable
    {
        #region private fields
        private bool mDisposed = false;
        private AppLifeCycleViewModel _AppLifeCycle = null;

        private bool _isInitialized = false;       // application should be initialized through one method ONLY!
        private object _lockObject = new object(); // thread lock semaphore

        private ICommand _ThemeSelectionChangedCommand = null;
        private ThemeViewModel _AppTheme = null;

        private FileViewModel _TargetFile = null;
        private ObservableCollection<FileInfoViewModel> _SourceFiles = null;
        private ObservableCollection<FileInfoViewModel> _SelectedSourceFileItems = null;

        private ProgressViewModel _Progress;

        private ICommand _BindPDFCommand = null;
        private ICommand _AddSourceFilesCommand = null;
        private ICommand _RemoveSourceFilesCommand = null;
        private ICommand _MoveUpSourceFilesCommand = null;
        private ICommand _MoveDownSourceFilesCommand = null;

        private ICommand _OpenContainingFolderCommand = null;
        private ICommand _OpenInWindowsCommand = null;
        private ICommand _CopyPathCommand = null;

        private ICommand _ToggleSettingsCommand;
        private bool _IsToggleSettingsChecked = false;

        private ICommand mSelectFolderCommand;
        private IBookmarksViewModel _BookmarkedLocation;
        #endregion private fields

        #region constructors
        /// <summary>
        /// Standard Constructor
        /// </summary>
        public AppViewModel(AppLifeCycleViewModel lifecycle)
            : this()
        {
            _AppLifeCycle = lifecycle;
        }

        /// <summary>
        /// Hidden standard constructor
        /// </summary>
        protected AppViewModel()
        {
            _AppTheme = new ThemeViewModel();

            _TargetFile = new FileViewModel();
            _SourceFiles = new ObservableCollection<FileInfoViewModel>();
            _SelectedSourceFileItems = new ObservableCollection<FileInfoViewModel>();

            _Progress = new ProgressViewModel();

            BookmarkedLocations = FileSystemModels.Factory.CreateBookmarksViewModel();

            BookmarkedLocations.AddFolder(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            BookmarkedLocations.AddFolder(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos));
            BookmarkedLocations.AddFolder(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
            BookmarkedLocations.AddFolder(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            BookmarkedLocations.AddFolder(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
            BookmarkedLocations.AddFolder(Environment.GetFolderPath(Environment.SpecialFolder.Windows));
            BookmarkedLocations.AddFolder(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles));
        }
        #endregion constructors

        #region properties
        public AppLifeCycleViewModel AppLifeCycle
        {
            get
            {
                return _AppLifeCycle;
            }
        }

        #region app theme
        /// <summary>
        /// Command executes when the user has selected
        /// a different UI theme to display.
        /// </summary>
        public ICommand ThemeSelectionChangedCommand
        {
            get
            {
                if (_ThemeSelectionChangedCommand == null)
                {
                    _ThemeSelectionChangedCommand = new RelayCommand<object>((p) =>
                    {
                        object[] paramets = p as object[];

                        if (Application.Current == null)
                            return;

                        if (Application.Current.MainWindow == null)
                            return;

                        if (paramets != null)
                        {
                            if (paramets.Length > 0)
                            {
                                _AppTheme.ApplyTheme(Application.Current.MainWindow,
                                                     (paramets[0] as IThemeInfo).DisplayName);
                            }
                        }
                    });
                }

                return _ThemeSelectionChangedCommand;
            }
        }

        /// <summary>
        /// Gets the currently selected application theme object.
        /// </summary>
        public ThemeViewModel AppTheme
        {
            get { return _AppTheme; }

            private set
            {
                if (_AppTheme != value)
                {
                    _AppTheme = value;
                    RaisePropertyChanged(() => this.AppTheme);
                }
            }
        }
        #endregion app theme

        /// <summary>
        /// Gets/sets the path and file name in which the target PDF file will be stored.
        /// </summary>
        public FileViewModel TargetFile
        {
            get { return _TargetFile; }
            set
            {
                if (_TargetFile != value)
                {
                    _TargetFile = value;
                    this.RaisePropertyChanged(() => this.TargetFile);
                }
            }
        }

        /// <summary>
        /// Gets a collection of all source files.
        /// </summary>
        public ObservableCollection<FileInfoViewModel> SourceFiles
        {
            get
            {
                return _SourceFiles;
            }
        }

        /// <summary>
        /// Exposes the currently selected sources files through an observable collection.
        /// </summary>
        public ObservableCollection<FileInfoViewModel> SelectedSourceFileItems
        {
            get
            {
                return _SelectedSourceFileItems;
            }
        }

        public ProgressViewModel Progress
        {
            get
            {
                return _Progress;
            }
        }

        public object CurrentViewModel
        {
            get
            {
                var vmManager = GetService<IVMManager>();
                var vm = vmManager.GetCurrentViewModel().Instance;

                return vm;
            }
        }

        public VMItem CurrentVMItem
        {
            get
            {
                var vmManager = GetService<IVMManager>();
                return vmManager.GetCurrentViewModel();
            }
        }

        
        public bool IsToggleSettingsChecked
        {
            get { return _IsToggleSettingsChecked; }
            set
            {
                if (_IsToggleSettingsChecked != value)
                {
                    _IsToggleSettingsChecked = value;
                    this.RaisePropertyChanged(() => this.IsToggleSettingsChecked);
                }
            }
        }

        #region Commands
        /// <summary>
        /// This command attempts to bind all source files
        /// into 1 collection of pages in one target PDF file.
        /// </summary>
        public ICommand BindPDFCommand
        {
            get
            {
                if (_BindPDFCommand == null)
                {
                    _BindPDFCommand = new RelayCommand(() =>
                    {
                        BindPDF(this.SourceFiles, this.TargetFile, this.Progress);
                    }, () =>
                    {
                        if (Progress.IsVisible == true)
                            return false;

                        if (string.IsNullOrEmpty(this.TargetFile.FileName) == true)
                            return false;

                        return true;
                    });
                }

                return _BindPDFCommand;
            }
        }

        /// <summary>
        /// Add source PDF file(s) from persistence storage into the
        /// collection of source files.
        /// </summary>
        public ICommand AddSourceFilesCommand
        {
            get
            {
                if (_AddSourceFilesCommand == null)
                {
                    _AddSourceFilesCommand = new RelayCommand(() =>
                    {
                        try
                        {
                            // Get a source or distination file path as default path
                            string defaultPath = this.GetDefaultPath(AppLifeCycleViewModel.MyDocumentsUserDir);

                            var pathCollection = new List<IFileItem>();
                            AppCommands.Instance.OnOpen("Select.PDFFile"
                                                       , defaultPath
                                                       , AppLifeCycleViewModel.MyDocumentsUserDir
                                                       , GetService<IFileManager>()
                                                       , GetService<IExplorer>()
                                                       , pathCollection
                                                       );

                            // Copy selected files into PDF source file collection
                            foreach (var item in pathCollection)
                                SourceFiles.Add(new FileInfoViewModel(item.Path, item.FileName));
                        }
                        catch (Exception exp)
                        {
                            var msg = GetService<IContentDialogService>().MsgBox;
                            msg.Show(exp, "Unexpected Error" // Local.Strings.STR_UnexpectedError_Caption
                                        , MsgBoxButtons.OK, MsgBoxImage.Error);
                        }
                    },
                    () =>
                    {
                        // Enable command if there is no background process being executed...
                        if (Progress.IsVisible == true)
                            return false;

                        return true;
                    });
                }

                return _AddSourceFilesCommand;
            }
        }

        public ICommand RemoveSourceFilesCommand
        {
            get
            {
                if (_RemoveSourceFilesCommand == null)
                {
                    _RemoveSourceFilesCommand = new RelayCommand<object>((p) =>
                    {
                        try
                        {
                            var objects = p as IList<object>;

                            if (objects != null)
                            {
                                for (int i = objects.Count - 1; i >= 0; i--)
                                {
                                    if (objects[i] is FileViewModel == true)
                                        this.SourceFiles.Remove(objects[i] as FileInfoViewModel);
                                }
                            }
                        }
                        catch (Exception exp)
                        {
                            var msg = GetService<IContentDialogService>().MsgBox;
                            msg.Show(exp, "Unexpected Error");
                        }
                    }, (p) =>
                    {
                        if (Progress.IsVisible == true)
                            return false;

                        return true;
                    });
                }

                return _RemoveSourceFilesCommand;
            }
        }

        public ICommand MoveUpSourceFilesCommand
        {
            get
            {
                if (_MoveUpSourceFilesCommand == null)
                {
                    _MoveUpSourceFilesCommand = new RelayCommand<object>((p) =>
                    {
                        try
                        {
                            var objects = p as IList<object>;

                            if (objects != null)
                            {
                                SortedList<int, int> idxs = new SortedList<int, int>();

                                for (int i = objects.Count - 1; i >= 0; i--)
                                {
                                    int idx = SourceFiles.IndexOf(objects[i] as FileInfoViewModel);
                                    idxs.Add(idx, idx);
                                }

                                // Items sorted in ascending order for move up to always work
                                // even when multiple items are selected
                                foreach (var item in idxs)
                                {
                                    if (item.Value > 0)
                                        SourceFiles.Move(item.Value, item.Value - 1);
                                }
                            }
                        }
                        catch (Exception exp)
                        {
                            var msg = GetService<IContentDialogService>().MsgBox;
                            msg.Show(exp, "Unexpected Error");
                        }
                    }, (p) =>
                    {
                        if (Progress.IsVisible == true)
                            return false;

                        return true;
                    });
                }

                return _MoveUpSourceFilesCommand;
            }
        }

        public ICommand ToggleSettingsCommand
        {
            get
            {
                if (_ToggleSettingsCommand == null)
                {
                    _ToggleSettingsCommand = new RelayCommand<object>((p) =>
                    {
                        toggleSettingsCommand();
                    });
                }

                return _ToggleSettingsCommand;
            }
        }

        public ICommand MoveDownSourceFilesCommand
        {
            get
            {
                if (_MoveDownSourceFilesCommand == null)
                {
                    _MoveDownSourceFilesCommand = new RelayCommand<object>((p) =>
                    {
                        try
                        {
                            var objects = p as IList<object>;

                            if (objects != null)
                            {
                                SortedList<int, int> idxs = new SortedList<int, int>();

                                for (int i = objects.Count - 1; i >= 0; i--)
                                {
                                    int idx = SourceFiles.IndexOf(objects[i] as FileInfoViewModel);

                                    // Sorting in everse order to opimize move down
                                    idxs.Add(idx * -1, idx);
                                }

                                // Items sorted in descending order for move down to always work
                                // even when multiple items are selected
                                foreach (var item in idxs)
                                {
                                    if (item.Value < SourceFiles.Count - 1)
                                        SourceFiles.Move(item.Value, item.Value + 1);
                                }
                            }
                        }
                        catch (Exception exp)
                        {
                            var msg = GetService<IContentDialogService>().MsgBox;
                            msg.Show(exp, "Unexpected Error");
                        }
                    }, (p) =>
                    {
                        if (Progress.IsVisible == true)
                            return false;

                        return true;
                    });
                }

                return _MoveDownSourceFilesCommand;
            }
        }

        /// <summary>
        /// Gets/sets a bookmark folder property to manage bookmarked folders.
        /// </summary>
        public IBookmarksViewModel BookmarkedLocations
        {
            get
            {
                return _BookmarkedLocation;
            }

            private set
            {
                if (_BookmarkedLocation != value)
                {
                    _BookmarkedLocation = value;
                    RaisePropertyChanged(() => BookmarkedLocations);
                }
            }
        }

        /// <summary>
        /// Gets a command to demo the folder browser dialog...
        /// </summary>
        public ICommand SelectFolderCommand
        {
            get
            {
                if (mSelectFolderCommand == null)
                {
                    mSelectFolderCommand = new RelayCommand<object>(async (p) =>
                    {
                        var initialPath = p as string;

                        if (string.IsNullOrEmpty(initialPath) == true)
                            initialPath = GetDefaultPath(AppLifeCycleViewModel.MyDocumentsUserDir);

                        var dlg = new FolderBrowserControler(initialPath, this.BookmarkedLocations);

                        var path = await dlg.ShowContentDialogFromVM(this, true);

                        if (string.IsNullOrEmpty(path) == false)
                            TargetFile.Path = path;

                        CloneBookMarks(dlg.BookmarkedLocations);

/***
                        // See Loaded event in FolderBrowserTreeView_Loaded methid to understand initial load
                        var treeBrowserVM = FolderBrowserFactory.CreateBrowserViewModel();

                        // Switch updates to view of by default to speed up load of view
                        // Loading the view will kick-off the browsing via View.Loaded Event
                        // and that in turn will switch on view updates ...
                        treeBrowserVM.UpdateView = false;

                        var initialPath = p as string;

                        if (string.IsNullOrEmpty(initialPath) == false)
                            treeBrowserVM.InitialPath = initialPath;
                        else
                            treeBrowserVM.InitialPath = this.GetDefaultPath(AppLifeCycleViewModel.MyDocumentsUserDir);

                        treeBrowserVM.SetSpecialFoldersVisibility(true);

                        var dlgVM = FolderBrowserFactory.CreateDialogViewModel(treeBrowserVM,
                                                                               BookmarkedLocations);

                        ////bool? bResult = dlg.ShowDialog();

                        bool? bResult = false;

                        var contentDlg = new Models.FBContentDialog.FolderBrowserContentDialog();
                        contentDlg.ShowAwaitCustomDialog(
                             MWindowDialogLib.Internal.Find.OwnerWindow(null) as IMetroWindow
                           , dlgVM
                           );

                        if (dlgVM.DialogCloseResult == true || bResult == true)
                        {
                            TargetFile.Path = dlgVM.TreeBrowser.SelectedFolder;

                            if (dlgVM.BookmarkedLocations != null)
                                this.BookmarkedLocations = dlgVM.BookmarkedLocations.CloneBookmark();
                        }
***/
                    });
                }

                return mSelectFolderCommand;
            }
        }

        #region Windows integration
        /// <summary>
        /// Open the  directory containing a given file in Windows Explorer.
        /// </summary>
        public ICommand OpenContainingFolderCommand
        {
            get
            {
                if (_OpenContainingFolderCommand == null)
                {
                    _OpenContainingFolderCommand = new RelayCommand<object>((p) =>
                    {
                        try
                        {
                            if (p is FileViewModel)
                            {
                                AppCommands.Instance.OnOpenContainingFolderCommand(
                                   (p as FileViewModel).FileName);
                            }
                        }
                        catch (Exception exp)
                        {
                            var msg = GetService<IContentDialogService>().MsgBox;
                            msg.Show(exp, "Unexpected Error");
                        }
                    });
                }

                return _OpenContainingFolderCommand;
            }
        }

        /// <summary>
        /// Open the currect file in an associated Windows application.
        /// </summary>
        public ICommand OpenInWindowsCommand
        {
            get
            {
                if (_OpenInWindowsCommand == null)
                {
                    _OpenInWindowsCommand = new RelayCommand<object>((p) =>
                    {
                        try
                        {
                            if (p is FileViewModel)
                            {
                                AppCommands.Instance.OnOpenInWindowsCommand(
                                   (p as FileViewModel).FileName);
                            }
                        }
                        catch (Exception exp)
                        {
                            var msg = GetService<IContentDialogService>().MsgBox;
                            msg.Show(exp, "Unexpected Error");
                        }
                    });
                }

                return _OpenInWindowsCommand;
            }
        }

        /// <summary>
        /// Copy the given file name and path into the Windows Clipboard.
        /// </summary>
        public ICommand CopyPathCommand
        {
            get
            {
                if (_CopyPathCommand == null)
                {
                    _CopyPathCommand = new RelayCommand<object>((p) =>
                    {
                        try
                        {
                            if (p is FileViewModel)
                            {
                                AppCommands.Instance.OnCopyFullPathtoClipboardCommand(
                                   (p as FileViewModel).FileName);
                            }
                        }
                        catch (Exception exp)
                        {
                            var msg = GetService<IContentDialogService>().MsgBox;
                            msg.Show(exp, "Unexpected Error");
                        }
                    });
                }

                return _CopyPathCommand;
            }
        }
        #endregion Windows integration
        #endregion Commands
        #endregion properties

        #region methods
        public VMItem SetCurrentViewModel(int itemKey)
        {
            var vmManager = GetService<IVMManager>();

            var item = vmManager.SetCurrentViewModel(VMItemKeys.ApplicationViewModel);

            RaisePropertyChanged(() => this.CurrentViewModel);
            RaisePropertyChanged(() => this.CurrentVMItem);

            return item;
        }

        #region Get/set Session Application Data
        internal void GetSessionData(IProfile sessionData)
        {
            if (sessionData.LastActiveTargetFile != TargetFile.FileName)
                sessionData.LastActiveTargetFile = TargetFile.FileName;

            sessionData.LastActiveSourceFiles = new List<SettingsModel.Models.FileReference>();
            if (SourceFiles != null)
            {
                foreach (var item in SourceFiles)
                    sessionData.LastActiveSourceFiles.Add(new SettingsModel.Models.FileReference()
                    { path = item.FileName }
                                                         );
            }
        }

        internal void SetSessionData(IProfile sessionData)
        {
            TargetFile.FileName = sessionData.LastActiveTargetFile;

            _SourceFiles = new ObservableCollection<FileInfoViewModel>();
            if (sessionData.LastActiveSourceFiles != null)
            {
                foreach (var item in sessionData.LastActiveSourceFiles)
                    _SourceFiles.Add(new FileInfoViewModel(item.path));
            }
        }
        #endregion Get/set Session Application Data

        /// <summary>
        /// Call this method if you want to initialize a headless
        /// (command line) application. This method will initialize only
        /// Non-WPF related items.
        /// 
        /// Method should not be called after <seealso cref="InitForMainWindow"/>
        /// </summary>
        public void InitWithoutMainWindow()
        {
            lock (_lockObject)
            {
                if (_isInitialized == true)
                    throw new Exception("AppViewModel initizialized twice.");

                _isInitialized = true;
            }
        }

        /// <summary>
        /// Call this to initialize application specific items that should be initialized
        /// before loading and display of mainWindow.
        /// 
        /// Invocation of This method is REQUIRED if UI is used in this application instance.
        /// 
        /// Method should not be called after <seealso cref="InitWithoutMainWindow"/>
        /// </summary>
        public void InitForMainWindow(IAppearanceManager appearance
                                      , string themeDisplayName)
        {
            // Initialize base that does not require UI
            InitWithoutMainWindow();

            appearance.AccentColorChanged += Appearance_AccentColorChanged;

            // Initialize UI specific stuff here
            this.AppTheme.ApplyTheme(Application.Current.MainWindow, themeDisplayName);
        }

        /// <summary>
        /// Standard dispose method of the <seealso cref="IDisposable" /> interface.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Source: http://www.codeproject.com/Articles/15360/Implementing-IDisposable-and-the-Dispose-Pattern-P
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (mDisposed == false)
            {
                if (disposing == true)
                {
                    // Dispose of the curently displayed content
                    ////mContent.Dispose();
                }

                // There are no unmanaged resources to release, but
                // if we add them, they need to be released here.
            }

            mDisposed = true;

            //// If it is available, make the call to the
            //// base class's Dispose(Boolean) method
            ////base.Dispose(disposing);
        }

        /// <summary>
        /// Method is invoked when theme manager is asked
        /// to change the accent color and has actually changed it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Appearance_AccentColorChanged(object sender, MLib.Events.ColorChangedEventArgs e)
        {

        }

        /// <summary>
        /// This command attempts to bind all source files
        /// into 1 collection of pages in one target PDF file.
        /// </summary>
        private async void BindPDF(IList<FileInfoViewModel> sourceFiles
                            , FileViewModel targetFile
                            , IProgress progress)
        {
            try
            {
                string resultPDF = string.Empty;

                // Bind all source PDF Files into 1 target file
                using (var combiner = BinderFactory.Instance.BindPDF())
                {
                    resultPDF = await combiner.BindPDFAsync(sourceFiles.ToArray<IPDFStateFile>(), targetFile, progress);
                }

                // Show resulting PDF File is any is available ...
                if (string.IsNullOrEmpty(resultPDF) == false)
                    AppCommands.Instance.OnOpenInWindowsCommand(resultPDF);

                progress.IsVisible = false;
            }
            catch (Exception exp)
            {
                var msg = GetService<IContentDialogService>().MsgBox;
                msg.Show(exp, "Unexpected Error" // Local.Strings.STR_UnexpectedError_Caption
                            , MsgBoxButtons.OK, MsgBoxImage.Error);

                progress.IsVisible = false;
            }
        }

        /// <summary>
        /// Toggles the settings viemodel to enable/disable editing settings.
        /// </summary>
        private void toggleSettingsCommand()
        {
            var vmManager = GetService<IVMManager>();

            var item = vmManager.GetCurrentViewModel();

            bool ToggleOn = true;

            if (item != null)
            {
                if (item.ItemKey == VMItemKeys.SettingsViewModel)
                    ToggleOn = false;
            }

            if (ToggleOn == true)
            {
                vmManager.AddVMItem(VMItemKeys.SettingsViewModel, "SettingsViewModel", new SettingsViewModel());
                vmManager.SetCurrentViewModel(VMItemKeys.SettingsViewModel);
                IsToggleSettingsChecked = false;
            }
            else
            {
                vmManager.SetCurrentViewModel(VMItemKeys.ApplicationViewModel);
                vmManager.RemoveVMItem(VMItemKeys.SettingsViewModel);
                IsToggleSettingsChecked = true;
            }

            RaisePropertyChanged(() => CurrentViewModel);
            RaisePropertyChanged(() => CurrentVMItem);
        }

        /// <summary>
        /// Get a source or distination file path as default path.
        /// Default path will be system default drive if no value is given.
        /// </summary>
        /// <param name="defaultPath"></param>
        /// <returns></returns>
        private string GetDefaultPath(string defaultPath = null)
        {
            if (defaultPath == null)
                defaultPath = PathFactory.SysDefault.Path;

            // Prefer target as default over source file collection
            try
            {
                if (System.IO.Directory.Exists(TargetFile.Path) == true)
                {
                    return TargetFile.Path;
                }
            }
            catch (Exception)
            {
            }

            if (SourceFiles.Count > 0)
            {
                try
                {
                    // Prefer last file in collection as default
                    for (int i = SourceFiles.Count - 1; i >= 0; i++)
                    {
                        if (System.IO.Directory.Exists(SourceFiles[i].Path) == true)
                            return SourceFiles[i].Path;
                    }
                }
                catch (Exception)
                {
                }
            }

            return defaultPath;
        }

        /// <summary>
        /// Method is invoked to copy the given bookmarks into the local
        /// bookmark locations object (typically called up close of browser dialog).
        /// </summary>
        /// <param name="bookmarkedLocations"></param>
        private void CloneBookMarks(IBookmarksViewModel bookmarkedLocations)
        {
            if (bookmarkedLocations == null)
                return;

            this.BookmarkedLocations = bookmarkedLocations.CloneBookmark();
        }
        #endregion methods
    }
}
