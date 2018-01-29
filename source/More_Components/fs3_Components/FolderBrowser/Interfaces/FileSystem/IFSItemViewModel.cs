
namespace FolderBrowser.FileSystem.Interfaces
{
    using FileSystemModels.Models.FSItems.Base;

    public interface IFSItemViewModel
    {
        System.Windows.Media.ImageSource DisplayIcon { get; }
        string DisplayName { get; }
        string FullPath { get; }
        PathModel GetModel { get; }
        int Indentation { get; }
        bool ShowToolTip { get; }
        FSItemType Type { get; }

        bool DirectoryPathExists();
        string DisplayItemString();
        void SetDisplayIcon(System.Windows.Media.ImageSource src = null);
    }
}
