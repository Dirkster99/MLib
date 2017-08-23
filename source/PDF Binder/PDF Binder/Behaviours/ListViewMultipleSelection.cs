namespace PDF_Binder.Behaviours
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;

    /// <summary>
    /// Implements an Attached Property to support DataGrid multiselection in an MVVM scenario.
    /// 
    /// Usage:
    /// <data:DataGrid ... SelectionMode="Extended" samples:DataGridMultipleSelection.SelectedItemsSource="{Binding SelectedProducts}" />
    /// <data:DataGrid ... SelectionMode="Extended" samples:DataGridMultipleSelection.SelectedItemsSource="{Binding SelectedProducts, Source={StaticResource CatalogModel}}" />
    /// Source: http://blogs.msdn.com/b/keithjones/archive/2009/10/02/multiple-selection-in-a-datagrid-should-be-bindable.aspx
    /// </summary>
    public static class ListViewMultipleSelection
    {
        /// <summary>
        /// Used in the selectedItemsSources private registry to hold state
        /// </summary>
        private class DataGridsAndInitiatedSelectionChange
        {
            public readonly List<WeakReference> BoundDataGridReferences = new List<WeakReference>();
            public bool InitiatedSelectionChange { get; set; }
        }

        // Use each source list's hashcode as the key so that we don't hold on
        // to any references in case the DataGrid gets disposed without telling
        // to remove the source list from our registry.
        private static Dictionary<int, DataGridsAndInitiatedSelectionChange> selectedItemsSources = new Dictionary<int, DataGridsAndInitiatedSelectionChange>();

        #region InitiatedSelectionChangeProperty
        /// <summary>
        /// For use only by this implementation to track if a DataGrid is the one changing the selection
        /// </summary>
        private static readonly DependencyProperty InitiatedSelectionChangeProperty = DependencyProperty.RegisterAttached("InitiatedSelectionChange", typeof(bool), typeof(ListViewMultipleSelection), new PropertyMetadata(false));

        /// <summary>
        /// Accessor to get the InitiatedSelectionChange DependencyProperty
        /// </summary>
        private static bool GetInitiatedSelectionChange(ListView element)
        {
            return (bool)element.GetValue(ListViewMultipleSelection.InitiatedSelectionChangeProperty);
        }

        /// <summary>
        /// Accessor to set the InitiatedSelectionChange DependencyProperty 
        /// </summary>
        private static void SetInitiatedSelectionChange(ListView element, bool value)
        {
            element.SetValue(ListViewMultipleSelection.InitiatedSelectionChangeProperty, value);
        }
        #endregion

        #region SelectedItemsSourceProperty

        /// <summary>
        /// Holds an IList implementing INotifyCollectionChanged to use as an items source for DataGrid.SelectedItems
        /// </summary>
        public static readonly DependencyProperty SelectedItemsSourceProperty = DependencyProperty.RegisterAttached("SelectedItemsSource", typeof(INotifyCollectionChanged), typeof(ListViewMultipleSelection), new PropertyMetadata(null, SelectedItemsSourceChanged));

        /// <summary>
        /// Accessor to get the SelectedItemsSource DependencyProperty
        /// </summary>
        public static INotifyCollectionChanged GetSelectedItemsSource(ListView element)
        {
            return element.GetValue(ListViewMultipleSelection.SelectedItemsSourceProperty) as INotifyCollectionChanged;
        }

        /// <summary>
        /// Accessor to set the SelectedItemsSource DependencyProperty
        /// </summary>
        public static void SetSelectedItemsSource(ListView element, INotifyCollectionChanged value)
        {
            element.SetValue(ListViewMultipleSelection.SelectedItemsSourceProperty, value);
        }

        /// <summary>
        /// Updates the items source registry when the SelectedItemsSource for a DataGrid changes
        /// </summary>
        private static void SelectedItemsSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            ListView dataGrid = sender as ListView;
            IList selectedItemsSource = null;

            // Check if the app is setting the source to a new or different list, or if it is removing the binding
            if (args.NewValue != null)
            {
                selectedItemsSource = args.NewValue as IList;
                if (selectedItemsSource == null)
                {
                    throw new ArgumentException("The value for SelectedItemsSource must implement IList.");
                }

                INotifyCollectionChanged collection = args.NewValue as INotifyCollectionChanged;
                if (collection == null)
                {
                    throw new ArgumentException("The value for SelectedItemsSource must implement INotifyCollectionChanged.");
                }

                // Don't add the event handler if the DataGrid is not setting its SelectedItemsSource for the first time
                if (args.OldValue == null)
                {
                    // Sign up for changes to the DataGrid's selected items to enable a two-way binding effect
                    dataGrid.SelectionChanged += UpdateSourceListOnDataGridSelectionChanged;
                }

                // Track this DataGrid instance for the specified source list
                DataGridsAndInitiatedSelectionChange sourceListInfo = null;
                if (ListViewMultipleSelection.selectedItemsSources.TryGetValue(selectedItemsSource.GetHashCode(), out sourceListInfo))
                {
                    sourceListInfo.BoundDataGridReferences.Add(new WeakReference(dataGrid));
                }
                else
                {
                    // This is a new source collection
                    sourceListInfo = new DataGridsAndInitiatedSelectionChange() { InitiatedSelectionChange = false };
                    sourceListInfo.BoundDataGridReferences.Add(new WeakReference(dataGrid));
                    ListViewMultipleSelection.selectedItemsSources.Add(selectedItemsSource.GetHashCode(), sourceListInfo);

                    // Sign up for changes to the source only on the first time the source is added
                    collection.CollectionChanged += UpdateDataGridsOnSourceCollectionChanged;
                }

                // Now force the DataGrid to update its SelectedItems to match the current
                // contents of the source list
                sourceListInfo.InitiatedSelectionChange = true;
                UpdateDataGrid(dataGrid, selectedItemsSource, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                sourceListInfo.InitiatedSelectionChange = false;
            }
            else
            {
                // This DataGrid is removing its SelectedItems binding to any list
                dataGrid.SelectionChanged -= UpdateSourceListOnDataGridSelectionChanged;
                dataGrid.SelectedItems.Clear();
            }

            if (args.OldValue != null)
            {
                // Clean up the items source that was the old value

                // Remove the DataGrid from the source list's registry and remove the source list
                // if there are no more DataGrids bound to it.
                DataGridsAndInitiatedSelectionChange sourceListInfo = ListViewMultipleSelection.selectedItemsSources[args.OldValue.GetHashCode()];
                WeakReference dataGridReferenceNeedingRemoval = null;
                foreach (WeakReference dataGridReference in sourceListInfo.BoundDataGridReferences)
                {
                    if (dataGridReference.IsAlive && (dataGridReference.Target == dataGrid))
                    {
                        dataGridReferenceNeedingRemoval = dataGridReference;
                        break;
                    }
                }
                sourceListInfo.BoundDataGridReferences.Remove(dataGridReferenceNeedingRemoval);
                if (sourceListInfo.BoundDataGridReferences.Count == 0)
                {
                    ListViewMultipleSelection.selectedItemsSources.Remove(args.OldValue.GetHashCode());

                    // Detach the event handlers and clear DataGrid.SelectedItems since the source is now null
                    INotifyCollectionChanged collection = args.OldValue as INotifyCollectionChanged;
                    if (collection != null)
                    {
                        collection.CollectionChanged -= UpdateDataGridsOnSourceCollectionChanged;
                    }
                }
            }
        }

        /// <summary>
        /// INotifyCollectionChanged.CollectionChanged handler for updating DataGrid.SelectedItems when the source list changes
        /// </summary>
        private static void UpdateDataGridsOnSourceCollectionChanged(object source, NotifyCollectionChangedEventArgs collectionChangedArgs)
        {
            DataGridsAndInitiatedSelectionChange sourceListInfo = ListViewMultipleSelection.selectedItemsSources[source.GetHashCode()];

            // For each DataGrid that is bound to this list, is alive, and did not initate selection changes, update its selection
            sourceListInfo.InitiatedSelectionChange = true;
            IList sourceList = source as IList;
            Debug.Assert(sourceList != null, "SelectedItemsSource must be of type IList");
            ListView dataGrid = null;
            foreach (WeakReference dataGridReference in sourceListInfo.BoundDataGridReferences)
            {
                if (dataGridReference.IsAlive && !ListViewMultipleSelection.GetInitiatedSelectionChange(dataGridReference.Target as ListView))
                {
                    dataGrid = dataGridReference.Target as ListView;
                    UpdateDataGrid(dataGrid, sourceList, collectionChangedArgs);
                }
            }
            sourceListInfo.InitiatedSelectionChange = false;
        }

        /// <summary>
        /// Helper method to update the items in DataGrid.SelectedItems based on the changes defined in the given NotifyCollectionChangedEventArgs
        /// </summary>
        /// <param name="dataGrid">DataGrid which owns the SelectedItems collection to update</param>
        /// <param name="sourceList">IList which is the SelectedItemsSource</param>
        /// <param name="collectionChangedArgs">The NotifyCollectionChangedEventArgs that was passed into the CollectionChanged event handler</param>
        private static void UpdateDataGrid(ListView dataGrid, IList sourceList, NotifyCollectionChangedEventArgs collectionChangedArgs)
        {
            switch (collectionChangedArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (object newItem in collectionChangedArgs.NewItems)
                    {
                        dataGrid.SelectedItems.Add(newItem);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (object oldItem in collectionChangedArgs.OldItems)
                    {
                        dataGrid.SelectedItems.Remove(oldItem);
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    // Unfortunately can not do the following two steps as an atomic change
                    // so the target list could raise multiple notifications as it gets updated
                    dataGrid.SelectedItems.Clear();
                    foreach (object item in sourceList)
                    {
                        dataGrid.SelectedItems.Add(item);
                    }
                    break;
                default:
                    throw new NotImplementedException("Only Add, Remove, and Reset actions are implemented.");
            }
        }

        /// <summary>
        /// DataGrid.SelectionChanged handler to update the source list given the SelectionChangedEventArgs
        /// </summary>
        private static void UpdateSourceListOnDataGridSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedArgs)
        {
            ListView dataGrid = sender as ListView;
            IList selectedItemsSource = ListViewMultipleSelection.GetSelectedItemsSource(dataGrid) as IList;
            Debug.Assert(selectedItemsSource != null, "SelectedItemsSource must be of type IList");

            // If the source list initiated the changes then don't pass the DataGrid's changes back down to the source list
            if (!ListViewMultipleSelection.selectedItemsSources[selectedItemsSource.GetHashCode()].InitiatedSelectionChange)
            {
                ListViewMultipleSelection.SetInitiatedSelectionChange(dataGrid, true);

                foreach (object removedItem in selectionChangedArgs.RemovedItems)
                {
                    selectedItemsSource.Remove(removedItem);
                }

                foreach (object addedItem in selectionChangedArgs.AddedItems)
                {
                    if (IsGenericList(selectedItemsSource.GetType(), addedItem.GetType()))
                        selectedItemsSource.Add(addedItem);
                }

                ListViewMultipleSelection.SetInitiatedSelectionChange(dataGrid, false);
            }
        }

        /// <summary>
        /// Compare a generic collection and determine whether its item-type matches
        /// the type of a given item.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="itemType"></param>
        /// <returns></returns>
        private static bool IsGenericList(Type type, Type itemType)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            foreach (Type @interface in type.GetInterfaces())
            {
                if (@interface.IsGenericType)
                {
                    // Match type used as generic argument with type of item
                    if (@interface.GenericTypeArguments[0] == itemType)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion
    }

    /****
     * 
     * A customer recently asked how to change the selection of a row in the Silverlight 3 DataGrid based on the values
     * of the columns. The brief answer is to add an event handler for LoadingRow and call dataGrid.SelectedItems.Add(e.Row.DataContext),
     * but the customer did mention that he was using M-V-VM and that got me thinking. The selection state of a DataGrid should be pulled
     * from the model, not calculated as part of the UI processing when the row gets loaded. The DataGrid supports this concept for
     * single-item selection because DataGrid.SelectedItem is data-bindable. The problem is that DataGrid.SelectedItems is pre-populated
     * with an internal collection which implements IList (the return type of DataGrid.SelectedItems). I turned to using attached properties
     * - the trusty duct-tape of Silverlight - to fill the gap.
     * 
     * Below is the C# code for a class containing an attached property called SelectedItemsSource which can be applied to a DataGrid to
     * keep its SelectedItems internal collection in-sync with a source list you specify. The synchronization is two-way and works with
     * multiple DataGrid instances synced to the same source list. The only limitation is that the source list must implement IList and
     * INotifyCollectionChanged. Disclaimer: This plays nice for the most part if the DataGrid.ItemsSource is bound to a collection implementing
     * ICollectionView (which handles single-item currency). In my limited testing this workaround falls down when you have multiple DataGrids
     * with the SelectedItemsSource attached property set to the same list and ItemsSource bound to the same ICollectionView instance.
     * 
     */
}
