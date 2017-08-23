namespace TreeViewDemo.Demos.ViewModels
{
    using Interfaces;
    using System.Collections.ObjectModel;
    using System;
    using System.Threading.Tasks;
    using System.Linq;
    using System.Windows;
    using System.Windows.Threading;

    /// <summary>
    /// Base class for all ViewModel classes displayed by TreeViewItems.  
    /// This acts as an adapter between a raw data object and a TreeViewItem.
    /// </summary>
    public class TreeViewItemViewModel : TreeViewDemo.ViewModels.Base.ViewModelBase, IFolder
    {
        #region fields
        static readonly IFolder DummyChild = new TreeViewItemViewModel();

        readonly ObservableCollection<IFolder> _children;
        readonly TreeViewItemViewModel _parent;
        readonly bool _LazyLoadChildren = true;

        private bool _isExpanded;
        private bool _isSelected;
        private string _Name;
        #endregion fields

        #region Constructors

        /// <summary>
        /// Cosntructs a new item in the tree.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name">Is the name of the item (eg.: 'C:\' or 'temp').</param>
        /// <param name="lazyLoadChildren"></param>
        protected TreeViewItemViewModel(TreeViewItemViewModel parent
                                      , string name
                                      , bool lazyLoadChildren
                                      , bool createDummyChild = true)
        {
            _parent = parent;
            _Name = name;
            _LazyLoadChildren = lazyLoadChildren;

            _children = new ObservableCollection<IFolder>();

            if (createDummyChild == true)
                this.ResetChildren(_LazyLoadChildren);
        }

        // This is used to create the DummyChild instance.
        protected TreeViewItemViewModel()
        {
        }
        #endregion // Constructors

        #region properties
        /// <summary>
        /// Returns the logical child items of this object.
        /// </summary>
        public ObservableCollection<IFolder> Children
        {
            get { return _children; }
        }

        #region IsExpanded

        /// <summary>
        /// Gets/sets whether the TreeViewItem 
        /// associated with this object is expanded.
        /// </summary>
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (value != _isExpanded)
                {
                    _isExpanded = value;
                    this.RaisePropertyChanged(() => IsExpanded);
                }

                // Expand all the way up to the root.
////                if (_isExpanded && _parent != null)
////                    _parent.IsExpanded = true;
            }
        }

        #endregion // IsExpanded

        #region IsSelected

        /// <summary>
        /// Gets/sets whether the TreeViewItem 
        /// associated with this object is selected.
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    this.RaisePropertyChanged(() => IsSelected);
                }
            }
        }

        #endregion // IsSelected

        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (value != _Name)
                {
                    _Name = value;
                    this.RaisePropertyChanged(() => Name);
                }
            }
        }

        public IFolder Parent
        {
            get { return _parent; }
        }
        #endregion properties

        #region methods
        public void SetExpand(bool isExpanded)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.IsExpanded = isExpanded;
            }),
            DispatcherPriority.Background, new object[0]);
        }

        public void SetSelect(bool isSelected)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.IsSelected = isSelected;
            }),
            DispatcherPriority.Background, new object[0]);
        }

        /// <summary>
        /// Returns true if this object's Children have not yet been populated.
        /// </summary>
        public virtual bool HasDummyChild
        {
            get
            {
                if (this.Children != null)
                {
                    if (this.Children.Count == 1)
                    {
                        if (this.Children[0] == DummyChild)
                            return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Returns a childs item reference based on its name or null.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IFolder FindChildByName(string name)
        {
            if (this.HasDummyChild == true || name == null)
                return null;

            return Children.SingleOrDefault(item => name == item.Name);
        }

        #region LoadChildren
        protected virtual void ResetChildren(bool lazyLoadChildren)
        {
            _children.Clear();

            if (lazyLoadChildren == true)
                _children.Add(DummyChild);
        }

        /// <summary>
        /// Invoked when the child items need to be loaded on demand.
        /// Subclasses can override this to populate the Children collection.
        /// </summary>
        protected virtual void LoadChildren()
        {
        }

        public Task<int> LoadChildrenAsync()
        {
            throw new NotImplementedException();
        }
        #endregion // LoadChildren
        #endregion methods
    }
}
