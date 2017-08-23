using System.Threading.Tasks;
using TreeViewDemo.Demos.ViewModels;

namespace TreeViewDemo.Demos.Interfaces
{
    public interface IFolder
    {
        #region properties
        /// <summary>
        /// Returns true if this object's Children have not yet been populated.
        /// </summary>
        bool HasDummyChild { get; }

        /// <summary>
        /// Gets the name (without the path) of this item.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets whether this folder is currently expanded or not.
        /// </summary>
        bool IsExpanded { get; }

        /// <summary>
        /// Get/set whether this folder is currently selected or not.
        /// </summary>
        bool IsSelected { get; }

        IFolder Parent { get; }
        #endregion methods

        Task<int> LoadChildrenAsync();

        /// <summary>
        /// Returns a childs item reference based on its name or null.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IFolder FindChildByName(string name);

        void SetExpand(bool isExpanded);
        void SetSelect(bool isExpanded);
    }
}
