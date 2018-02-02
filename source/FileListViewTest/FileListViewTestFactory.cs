namespace FileListViewTest
{
    using FileListViewTest.Interfaces;
    using FileListViewTest.ViewModels;

    public sealed class FileListViewTestFactory
    {
        private FileListViewTestFactory()
        { }

        /// <summary>
        /// Creates a <see cref="IControllerListViewModel"/> instance and returns it.
        /// </summary>
        /// <returns></returns>
        public static IControllerListViewModel Create()
        {
            return new ControllerListViewModel();
        }
    }
}
