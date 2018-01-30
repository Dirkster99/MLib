namespace TreeViewDemo.Demos.ViewModels
{
    using FileSystemModels;
    using FileSystemModels.Interfaces;
    using FileSystemModels.Models.FSItems.Base;
    using Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Threading;

    public class FolderViewModel : TreeViewItemViewModel, IFolder
    {
        private readonly IPathModel _folder;

        public FolderViewModel(IPathModel folder, TreeViewItemViewModel folderParent)
            : base(folderParent, folder.Name, true)
        {
            _folder = folder;
        }

        protected override void LoadChildren()
        {
            this.Children.Clear();

            foreach (var item in PathFactory.GetDirectories(_folder.Name))
            {
                base.Children.Add(new FolderViewModel(PathFactory.Create(item, FSItemType.Folder), this));
            }
        }

        public async new Task<int> LoadChildrenAsync()
        {
            var folderVMItems = await FolderViewModel.LoadSubFolderAsync(_folder.Path, this);
            var paramObject = new object[0];

            if (folderVMItems.Count == 0)
            {
                this.Children.Clear();
            }
            else
            {
                await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.Children[0] = folderVMItems[0];
                }),
                DispatcherPriority.Background, paramObject);

                for (int i = 1; i < folderVMItems.Count; i++)
                {
                    await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        base.Children.Add(folderVMItems[i]);
                    }),
                    DispatcherPriority.Background, paramObject);
                }
            }

            return Children.Count;
        }

        internal async static Task<List<FolderViewModel>> LoadSubFolderAsync(string path, TreeViewItemViewModel parent)
        {
            var items = await PathFactory.LoadFoldersAsync(path);
            var viewmodelItems = new List<FolderViewModel>();

            foreach (var item in items)
                viewmodelItems.Add(new FolderViewModel(item, parent));

            return viewmodelItems;
        }
    }
}
