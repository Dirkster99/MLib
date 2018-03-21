namespace MLib.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Media;

    
    /// <summary>
    /// Converts a treeviewitem (and its computed depth) into a margin value.
    /// </summary>
    public class TreeViewMarginConverter : IValueConverter
    {
        /// <summary>
        /// Gets/sets default margin size in dependence of depth of a <seealso cref="TreeViewItem"/>.
        /// </summary>
        public double Length { get; set; }

        /// <summary>
        /// Converts a treeviewitem (and its computed depth) into a margin.
        /// </summary>
        /// <param name = "value"></param>
        /// <param name = "targetType"></param>
        /// <param name = "parameter"></param>
        /// <param name = "culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var item = value as TreeViewItem;

            if (item == null)
                return new Thickness(0);

            return new Thickness(Length * item.GetDepth(), 0, 0, 0);
        }

        /// <summary>
        /// Method is not implemented.
        /// </summary>
        /// <param name = "value"></param>
        /// <param name = "targetType"></param>
        /// <param name = "parameter"></param>
        /// <param name = "culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    /// <summary>
    /// Provides extension methods for treeview items.
    /// </summary>
    public static class TreeViewItemExtensions
    {
        /// <summary>
        /// Gets the level (depth) of a tree view item and returns it as integer value.
        /// </summary>
        /// <param name = "item"></param>
        /// <returns>the depth of treeviewitem as integer value</returns>
        public static int GetDepth(this TreeViewItem item)
        {
            TreeViewItem parent;
            while ((parent = GetParent(item)) != null)
            {
                return GetDepth(parent) + 1;
            }
            return 0;
        }

        /// <summary>
        /// Gets the parent of a tree view via search in visual tree.
        /// </summary>
        /// <param name = "item"></param>
        /// <returns>the parent if any</returns>
        private static TreeViewItem GetParent(TreeViewItem item)
        {
            var parent = item != null ? VisualTreeHelper.GetParent(item) : null;
            while (parent != null && !(parent is TreeViewItem || parent is TreeView))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            return parent as TreeViewItem;
        }
    }
}
