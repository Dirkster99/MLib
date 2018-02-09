namespace FileListViewTest
{
    using FileListViewTest.Interfaces;
    using FileListViewTest.ViewModels;

    public sealed class FileListViewTestFactory
    {
        private FileListViewTestFactory()
        { }

        /// <summary>
        /// Creates a <see cref="IListControllerViewModel"/> instance and returns it.
        /// </summary>
        /// <returns></returns>
        public static IListControllerViewModel CreateList()
        {
            return new ListControllerViewModel();
        }

        public static ITreeListControllerViewModel CreateTreeList()
        {
            return new TreeListControllerViewModel();
        }
    }
}
