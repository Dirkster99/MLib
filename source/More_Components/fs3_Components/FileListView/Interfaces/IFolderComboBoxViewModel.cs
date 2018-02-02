namespace FileListView.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Input;
    using FileListView.Interfaces;
    using FileSystemModels.Events;

    public interface IFolderComboBoxViewModel
    {
        string CurrentFolder { get; set; }
        string CurrentFolderToolTip { get; }
        IEnumerable<ILVItemViewModel> CurrentItems { get; }
        ILVItemViewModel SelectedItem { get; set; }
        ICommand SelectionChanged { get; }

        event EventHandler<FolderChangedEventArgs> RequestChangeOfDirectory;

        void PopulateView();
    }
}