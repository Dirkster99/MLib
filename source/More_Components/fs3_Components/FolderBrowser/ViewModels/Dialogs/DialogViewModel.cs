namespace FolderBrowser.Dialogs.ViewModels
{
    using FolderBrowser.Dialogs.Interfaces;
    using FolderBrowser.Interfaces;
    using FolderBrowser.ViewModels;
    using MWindowInterfacesLib.MsgBox;

    /// <summary>
    /// A dialog viewmodel in MVVM style to drive a folder browser
    /// or folder picker dialog with a WPF view.
    /// </summary>
    internal class DialogViewModel : DialogBaseViewModel, IDialogViewModel
    {
        #region fields
        private bool? mDialogCloseResult = null;
        #endregion fields

        /// <summary>
        /// Class constructor
        /// </summary>
        public DialogViewModel(IMessageBoxService msgBox,
                               IBrowserViewModel treeBrowser = null,
                               IBookmarkedLocationsViewModel recentLocations = null)
            : base (msgBox, treeBrowser, recentLocations)
        {
        }

        /// <summary>
        /// This can be used to close the attached view via ViewModel
        /// 
        /// Source: http://stackoverflow.com/questions/501886/wpf-mvvm-newbie-how-should-the-viewmodel-close-the-form
        /// </summary>
        public bool? DialogCloseResult
        {
            get
            {
                return mDialogCloseResult;
            }

            private set
            {
                if (mDialogCloseResult != value)
                {
                    mDialogCloseResult = value;
                    RaisePropertyChanged(() => DialogCloseResult);
                }
            }
        }
    }
}
