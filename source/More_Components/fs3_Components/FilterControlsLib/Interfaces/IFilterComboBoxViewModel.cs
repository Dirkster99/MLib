namespace FilterControlsLib.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Input;
    using FileSystemModels.Events;

    public interface IFilterComboBoxViewModel
    {
        event EventHandler<FilterChangedEventArgs> OnFilterChanged;

        #region properties
        string CurrentFilter { get; set; }
        string CurrentFilterToolTip { get; }
        IEnumerable<IFilterItemViewModel> CurrentItems { get; }
        IFilterItemViewModel SelectedItem { get; set; }
        ICommand SelectionChanged { get; }
        #endregion properties

        #region methods
        void AddFilter(string filterString, bool bSelectNewFilter = false);
        void AddFilter(string name, string filterString, bool bSelectNewFilter = false);

        void ClearFilter();

        IEnumerable<IFilterItemViewModel> FindFilter(string name, string filterString);
        void SetCurrentFilter(string filterDisplayName, string filterText);
        #endregion methods
    }
}