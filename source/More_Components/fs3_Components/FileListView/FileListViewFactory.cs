namespace FileListView
{
    using FileListView.ViewModels;
    using FileListView.ViewModels.Interfaces;

    public sealed class FileListViewFactory
    {
        private FileListViewFactory()
        { }

        /// <summary>
        /// Creates a <see cref="IFolderListViewModel"/> instance and returns it.
        /// </summary>
        /// <returns></returns>
        public static IFolderListViewModel Create()
        {
            return new ControllerListViewModel();
        }
    }
}
