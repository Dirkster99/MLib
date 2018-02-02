namespace FileListView.Interfaces
{
    using FileSystemModels.Interfaces;
    using FileSystemModels.Models.FSItems.Base;
    using System.Windows.Media;

    public interface ILVItemViewModel
    {
        ImageSource DisplayIcon { get; }
        string DisplayName { get; }
        string FullPath { get; }
        IPathModel GetModel { get; }
        int Indentation { get; }
        bool ShowToolTip { get; }
        FSItemType Type { get; }

        bool DirectoryPathExists();
        string DisplayItemString();
        void RenameFileOrFolder(string newFolderName);
        void SetDisplayIcon(ImageSource src = null);
        string ToString();
    }
}