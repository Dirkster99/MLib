namespace FileListViewTest.Interfaces
{
    using FolderBrowser.Interfaces;

    /// <summary>
    /// Interface implements a folder/file view model class
    /// that can be used to dispaly filesystem related content in an ItemsControl.
    /// </summary>
    public interface ITreeListControllerViewModel : IListControllerViewModel
    {
        IBrowserViewModel TreeBrowser { get; }
    }
}
