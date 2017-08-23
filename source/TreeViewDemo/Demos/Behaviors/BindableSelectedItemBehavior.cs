﻿namespace TreeViewDemo.Demos.Behaviors
{
    using Interfaces;
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Interactivity;

    /// <summary>
    /// Allows two-way binding a TreeView's selected item.
    /// </summary>
    public class BindableSelectedItemBehavior : Behavior<TreeView>
    {
        public IFolder[] SelectedItem
        {
            get { return (IFolder[])GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem",
                typeof(IFolder[]),
                typeof(BindableSelectedItemBehavior),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemChanged));

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var newNode = e.NewValue as IFolder[];
            if (newNode == null)
                return;

            if (newNode.Length <= 1)
                return;

            var behavior = (BindableSelectedItemBehavior)d;
            var tree = behavior.AssociatedObject;

            ////    var nodeDynasty = new List<IFolder> { newNode };
            ////    var parent = newNode.Parent;
            ////    while (parent != null)
            ////    {
            ////        nodeDynasty.Insert(0, parent);
            ////        parent = parent.Parent;
            ////    }

            var currentParent = tree as ItemsControl;
            for (int i = 0; i < newNode.Length; i++)
            {
                var node = newNode[i];

                // first try the easy way
                var newParent = currentParent.ItemContainerGenerator.ContainerFromItem(node) as TreeViewItem;
                if (newParent == null)
                {
                    // if this failed, it's probably because of virtualization, and we will have to do it the hard way.
                    // this code is influenced by TreeViewItem.ExpandRecursive decompiled code, and the MSDN sample at http://code.msdn.microsoft.com/Changing-selection-in-a-6a6242c8/sourcecode?fileId=18862&pathId=753647475
                    // see also the question at http://stackoverflow.com/q/183636/46635
                    currentParent.ApplyTemplate();
                    var itemsPresenter = (ItemsPresenter)currentParent.Template.FindName("ItemsHost", currentParent);
                    if (itemsPresenter != null)
                    {
                        itemsPresenter.ApplyTemplate();
                    }
                    else
                    {
                        currentParent.UpdateLayout();
                    }

                    var virtualizingPanel = GetItemsHost(currentParent) as VirtualizingPanel;

                    CallEnsureGenerator(virtualizingPanel);
                    var index = currentParent.Items.IndexOf(node);
                    if (index < 0)
                    {
                        throw new InvalidOperationException("Node '" + node + "' cannot be fount in container");
                    }
                    virtualizingPanel.BringIndexIntoViewPublic(index);
                    newParent = currentParent.ItemContainerGenerator.ContainerFromIndex(index) as TreeViewItem;
                }

                if (newParent == null)
                {
                    throw new InvalidOperationException("Tree view item cannot be found or created for node '" + node + "'");
                }

                if (node == newNode[newNode.Length-1])
                {
                    newParent.IsSelected = true;
                    newParent.BringIntoView();
                    break;
                }

                newParent.IsExpanded = true;
                currentParent = newParent;
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectedItemChanged += OnTreeViewSelectedItemChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.SelectedItemChanged -= OnTreeViewSelectedItemChanged;
        }

        private void OnTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedItem = e.NewValue as IFolder[];
        }

        #region Functions to get internal members using reflection
        // Some functionality we need is hidden in internal members, so we use reflection to get them
        #region ItemsControl.ItemsHost

        static readonly PropertyInfo ItemsHostPropertyInfo = typeof(ItemsControl).GetProperty("ItemsHost", BindingFlags.Instance | BindingFlags.NonPublic);

        private static Panel GetItemsHost(ItemsControl itemsControl)
        {
            Debug.Assert(itemsControl != null);
            return ItemsHostPropertyInfo.GetValue(itemsControl, null) as Panel;
        }

        #endregion ItemsControl.ItemsHost

        #region Panel.EnsureGenerator
        private static readonly MethodInfo EnsureGeneratorMethodInfo = typeof(Panel).GetMethod("EnsureGenerator", BindingFlags.Instance | BindingFlags.NonPublic);

        private static void CallEnsureGenerator(Panel panel)
        {
            Debug.Assert(panel != null);
            EnsureGeneratorMethodInfo.Invoke(panel, null);
        }
        #endregion Panel.EnsureGenerator
        #endregion Functions to get internal members using reflection
    }
}
