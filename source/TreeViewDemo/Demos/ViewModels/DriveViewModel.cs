namespace TreeViewDemo.Demos.ViewModels
{
    using FileSystemModels;
    using FileSystemModels.Interfaces;
    using FileSystemModels.Models.FSItems.Base;
    using Interfaces;
    using System;
    using System.Threading.Tasks;
    using System.Windows;

    public class DriveViewModel : TreeViewItemViewModel, IFolder
    {
        private readonly IPathModel _Model;

        public DriveViewModel(IPathModel _model
                            , ComputerViewModel computerParent)
            : base(computerParent, _model.Name, true)
        {
            _Model = _model;
        }

        protected override void LoadChildren()
        {
            foreach (var item in PathFactory.GetDirectories(_Model.Name))
            {
                base.Children.Add(new FolderViewModel(PathFactory.Create(item, FSItemType.Folder), this));
            }
        }

        public async new Task<int> LoadChildrenAsync()
        {
            this.Children.Clear();
            var folderVMItems = await FolderViewModel.LoadSubFolderAsync(_Model.Path, this);

            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                foreach (var item in folderVMItems)
                    base.Children.Add(item);
            }));

            return Children.Count;
        }
    }
}
