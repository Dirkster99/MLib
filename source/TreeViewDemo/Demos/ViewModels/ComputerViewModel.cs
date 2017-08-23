namespace TreeViewDemo.Demos.ViewModels
{
    using Models;
    using Models.FSItems;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Threading;
    using Interfaces;

    public class ComputerViewModel : TreeViewItemViewModel
    {
        public ComputerViewModel()
            : base(null, "My Computer", true)
        {
        }

        public async new Task<int> LoadChildrenAsync()
        {
            var drives = await DriveModel.GetLogicalDrivesAsync();

            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                foreach (var drive in drives)
                    base.Children.Add(new DriveViewModel(drive, this));
            }),
            DispatcherPriority.Background, new object[0]);

            return drives.Count();
        }

        public async Task<bool> InitRootAsync()
        {
            ResetChildren(false);          // Reset collection of Children without dummy child item.

            await LoadChildrenAsync();    // Load Root items below the Computer item
            this.IsExpanded = true;       // Make Children (drives) visible

////            bool exists = await PathModel.DirectoryPathExistsAsync(path);
////
////            if (exists == false)
////                return false;

            return true;
        }

        internal async Task<IFolder[]> BrowsePath(string inputPath)
        {
            try
            {
                // Check if a given path exists
                var exists = await PathModel.DirectoryPathExistsAsync(inputPath);

                if (exists == false)
                    return null;

                // Transform string into array of normalized path elements
                // Drive 'C:\' , 'Folder', 'SubFolder', etc...
                //
                var folders = PathModel.GetDirectories(inputPath);

                // Find the drive that is the root of this path
                var drive = this.FindChildByName(folders[0]);

                return await NavigatePathAsync(drive, folders);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<IFolder[]> NavigatePathAsync(IFolder parent
                                           , string[] folders
                                           , int iMatchIdx = 0)
        {
            IFolder[] pathFolders = new IFolder[folders.Count()+1];

            pathFolders[0] = this;
            pathFolders[1] = parent;

            int iNext = iMatchIdx + 1;
            for ( ; iNext < folders.Count(); iNext++)
            {
                if(parent.HasDummyChild == true)
                    await parent.LoadChildrenAsync();

                var nextChild = parent.FindChildByName(folders[iNext]);

                if (nextChild != null)
                {
                    pathFolders[iNext+1] = nextChild;
                    parent = nextChild;
                }
            }

////            iNext--;
////            var selectItem = pathFolders[iNext].FindChildByName(folders[folders.Count() - 1]);
////
////            for (  ; iNext > 0; iNext--)
////            {
////                pathFolders[iNext].SetExpand(true);
////            }

            return pathFolders; // selectItem;
        }
    }
}
