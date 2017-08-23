namespace FsContentDialogDemo.Demos
{
    using FolderBrowser;
    using FolderBrowser.Dialogs.Interfaces;
    using MWindowDialogLib.Dialogs;
    using MWindowInterfacesLib.Interfaces;
    using System.Threading.Tasks;

    public class FolderBrowserControler : FsContentDialogDemo.ViewModels.Base.ModelBase
    {
        #region Fields
        private bool _SpecialFolderVisibility;
        private string _InitialPath;
        private IBookmarkedLocationsViewModel _FBB_Bookmarks;
        #endregion Fields

        #region ctor
        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="initialPath"></param>
        /// <param name="bookmarks"></param>
        /// <param name="specialFolderVisibility"></param>
        public FolderBrowserControler(string initialPath
                                     , IBookmarkedLocationsViewModel bookmarks
                                     , bool specialFolderVisibility = true)
            : this()
        {
            _SpecialFolderVisibility = specialFolderVisibility;
            _InitialPath = initialPath;

            if (bookmarks != null)
                _FBB_Bookmarks = bookmarks.Copy();
        }

        /// <summary>
        /// Default Class Constructor
        /// </summary>
        protected FolderBrowserControler()
        {
            _SpecialFolderVisibility = true;
            _InitialPath = string.Empty;
            _FBB_Bookmarks = null;
        }
        #endregion ctor

        #region methods
        /// <summary>
        /// Shows a sample progress dialog that was invoked via a bound viewmodel.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="progressIsFinite"></param>
        internal async Task<string> ShowContentDialogFromVM(
              object context
            , bool progressIsFinite
            )
        {
            var msgBox = GetService<IContentDialogService>().MsgBox;

            // See Loaded event in FolderBrowserTreeView_Loaded method to understand initial load
            var treeBrowserVM = FolderBrowserFactory.CreateBrowserViewModel(msgBox
                                                                          , _SpecialFolderVisibility
                                                                          , _InitialPath);

            // Switch updates to view of by default to speed up load of view
            // Loading the view will kick-off the browsing via View.Loaded Event
            // and that in turn will switch on view updates ...
            treeBrowserVM.UpdateView = false;

            var fsDlg = FolderBrowserFactory.CreateDialogViewModel(msgBox, treeBrowserVM, _FBB_Bookmarks);

            var customDialog = CreatBrowseProgressDialog(new ViewModels.FolderBrowserContentDialogViewModel(fsDlg));

            var coord = GetService<IContentDialogService>().Coordinator;
            var manager = GetService<IContentDialogService>().Manager;

            string returnPath = null;

            // Show a progress dialog to initialize the viewmodel - in case file system is slow...
            await coord.ShowMetroDialogAsync(context, customDialog).ContinueWith
            (
                (t) =>
                {
                    if (t.Result == DialogIntResults.OK)
                        returnPath = treeBrowserVM.SelectedFolder;
                }
            );

            return returnPath;
        }

        /// <summary>
        /// Creates a <seealso cref="MWindowDialogLib.Dialogs.CustomDialog"/> that contains a <seealso cref="ProgressView"/>
        /// in its content and has a <seealso cref="ProgressViewModel"/> attached to its datacontext,
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        private CustomDialog CreatBrowseProgressDialog(object viewModel)
        {
            var dlg = new CustomDialog(new Demos.Views.FolderBrowserContentDialogView(),viewModel);

            // Strech dialog to always use complete space since dialog will otherwise
            // flip open/shut/flicker when browsing beween small path 'c:\'
            // and deep/long path like "c:\windows\system32"
            dlg.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            dlg.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;

            return dlg;
        }
        #endregion methods
    }
}
