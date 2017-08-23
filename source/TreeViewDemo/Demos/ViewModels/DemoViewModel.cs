using System.Collections.ObjectModel;
using System.Windows.Input;
using TreeViewDemo.Demos.Interfaces;
using TreeViewDemo.ViewModels.Base;

namespace TreeViewDemo.Demos.ViewModels
{
    public class DemoViewModel : TreeViewDemo.ViewModels.Base.ViewModelBase
    {
        private IFolder _SelectedItem = null;

        private const string _DemoPath = @"C:\Program Files\MSBuild\Microsoft\Windows Workflow Foundation\v3.0";

        private readonly ComputerViewModel _ComputerInstance = null;
        private readonly ReadOnlyCollection<ComputerViewModel> _computer = null;

        private ICommand _InitRootCommand;
        private ICommand _ExpandCommand;
        private ICommand _BrowsePathCommand;
        private IFolder[] _SelectPathItem;

        public DemoViewModel()
        {
            _SelectPathItem = null;
            _ComputerInstance = new ComputerViewModel();
            _computer = new ReadOnlyCollection<ComputerViewModel>(new ComputerViewModel[]
                                                                  {
                                                                      _ComputerInstance
                                                                  });
        }

        public ReadOnlyCollection<ComputerViewModel> Computer
        {
            get
            {
                return _computer;
            }
        }

        public ICommand InitRootCommand
        {
            get
            {
                if (_InitRootCommand == null)
	            {
                    _InitRootCommand = new RelayCommand<object>(async (p) =>
                    {
                        await _ComputerInstance.InitRootAsync();
                    });
                }

                return _InitRootCommand;
            }
        }

        public ICommand ExpandCommand
        {
            get
            {
                if (_ExpandCommand == null)
                {
                    _ExpandCommand = new RelayCommand<object>(async (p) =>
                    {
                        var folder = p as IFolder;

                        if (folder == null)
                            return;

                        await folder.LoadChildrenAsync();
                    });
                }

                return _ExpandCommand;
            }
        }

        public ICommand BrowsePathCommand
        {
            get
            {
                if (_BrowsePathCommand == null)
                {
                    _BrowsePathCommand = new RelayCommand<object>(async (p) =>
                    {
                        var selItem = await _ComputerInstance.BrowsePath(_DemoPath);

                        if (selItem != null)
                        {
                            //this.SelectedItem = selItem;
                            this.SelectPathItem = selItem;
                        }
                    });
                }

                return _BrowsePathCommand;
            }
        }

        public IFolder SelectedItem
        {
            get
            {
                return _SelectedItem;
            }
            set
            {
                if (value != _SelectedItem)
                {
                    _SelectedItem = value;
                    this.RaisePropertyChanged(() => SelectedItem);
                }

                // Expand all the way up to the root.
                ////                if (_isExpanded && _parent != null)
                ////                    _parent.IsExpanded = true;
            }
        }

        public IFolder[] SelectPathItem
        {
            get
            {
                return _SelectPathItem;
            }

            set
            {
                if (value != _SelectPathItem)
                {
                    _SelectPathItem = value;
                    this.RaisePropertyChanged(() => SelectPathItem);
                }
            }
        }
    }
}
