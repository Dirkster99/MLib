namespace TreeViewDemo.Demos.ViewModels
{
    using FileSystemModels.Models.FSItems.Base;
    using Interfaces;
    using System;
    using System.Threading.Tasks;
    using System.Windows;

    public class DriveViewModel : TreeViewItemViewModel, IFolder
    {
        readonly PathModel _Model;

        public DriveViewModel(PathModel _model
                            , ComputerViewModel computerParent)
            : base(computerParent, _model.Name, true)
        {
            _Model = _model;
        }

        protected override void LoadChildren()
        {
            foreach (var item in PathModel.GetDirectories(_Model.Name))
            {
                base.Children.Add(new FolderViewModel(new PathModel(item, FSItemType.Folder), this));
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
