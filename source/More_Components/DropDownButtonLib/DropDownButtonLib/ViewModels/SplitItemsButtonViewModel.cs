namespace DropDownButtonLib.ViewModels
{
  using System;
  using System.Collections.ObjectModel;
  using System.Windows.Input;
  using DropDownButtonLib.Command;
  using DropDownButtonLib.ViewModels.Items;

  /// <summary>
  /// Implements a viewmodel for a split button that can display multiple items.
  /// The user experience is similar to a combobox with the difference that a
  /// splitbutton allows a re-apply of the last selected item.
  /// </summary>
  public class SplitItemsButtonViewModel : Base.BaseViewModel
  {
    #region fields
    private readonly ObservableCollection<ItemsItemViewModel> mDropDownItems;

    private RelayCommand<object> mDropDownButtonClickCommand;
    private RelayCommand<object> mItemButtonClickCommand;

    private bool mIsOpen;
    private string mStatus;
    private bool mIsEnabled;
    private ItemsItemViewModel mSelectedItem;
    #endregion fields

    #region constructor
    /// <summary>
    /// Class constructor
    /// </summary>
    public SplitItemsButtonViewModel()
    {
      this.mIsOpen = false;
      this.mIsEnabled = true;
      this.mStatus = string.Empty;
      this.mDropDownItems = new ObservableCollection<ItemsItemViewModel>();

      this.mSelectedItem = new ItemsItemViewModel("Drop Down Items Demo 1");

      this.mDropDownItems.Add(this.mSelectedItem);

      for (int i = 2; i < 255; i++)
        this.mDropDownItems.Add(new ItemsItemViewModel("Drop Down Items Demo " + i));
    }
    #endregion constructor

    #region properties
    /// <summary>
    /// Command is invoked when user clicks on the DropDownButton
    /// (either on label part or DropDown arrow).
    /// </summary>
    public ICommand DropDownButtonClickCommand
    {
      get
      {
        if (this.mDropDownButtonClickCommand == null)
          this.mDropDownButtonClickCommand = new RelayCommand<object>(
            (p) => 
            {
              var param = p as ItemsItemViewModel;

              if (param != null)
                this.DropDownButtonClickCommand_Executed(param);
            },
            (p) => this.IsEnabled);

        return this.mDropDownButtonClickCommand;
      }
    }

    /// <summary>
    /// Command is invoked when user clicks on the DropDownButton
    /// (user clicked a button or hyperlink in DropDown part).
    /// </summary>
    public ICommand ItemButtonClickCommand
    {
      get
      {
        if (this.mItemButtonClickCommand == null)
          this.mItemButtonClickCommand = new RelayCommand<object>((p) =>
          {
            var param = p as ItemsItemViewModel;

            if (param != null)
            this.ItemButtonClickCommand_Executed(param);
          },
          (p) => this.IsEnabled);

        return this.mItemButtonClickCommand;
      }
    }

    /// <summary>
    /// Gets the list of drives and folders for display in treeview structure control.
    /// </summary>
    public ObservableCollection<ItemsItemViewModel> DropDownItems
    {
      get
      {
        return this.mDropDownItems;
      }
    }

    /// <summary>
    /// Gets/sets the selected (current) item of this button viewmodel.
    /// </summary>
    public ItemsItemViewModel SelectedItem
    {
      get
      {
        return this.mSelectedItem;
      }

      set
      {
        if (this.mSelectedItem != value)
        {
          this.mSelectedItem = value;
          this.NotifyPropertyChanged(() => this.SelectedItem);
        }
      }
    }

    /// <summary>
    /// Gets/sets bound property to determine whether
    /// drop-down/pop-up element is open or not.
    /// </summary>
    public bool IsOpen
    {
      get
      {
        return this.mIsOpen;
      }

      set
      {
        if (this.mIsOpen != value)
        {
          this.mIsOpen = value;
          this.NotifyPropertyChanged(() => this.IsOpen);
        }
      }
    }

    /// <summary>
    /// Gets/sets Boolean value to determine whether attached dropdown view is enabled or not.
    /// </summary>
    public bool IsEnabled
    {
      get
      {
        return this.mIsEnabled;
      }

      set
      {
        if (this.mIsEnabled != value)
        {
          this.mIsEnabled = value;
          this.NotifyPropertyChanged(() => this.IsEnabled);
        }
      }
    }

    /// <summary>
    /// Gets the status description of this viewmodel
    /// (this property is present for testing/demo/debugging purposes only
    /// since it gives us the ability to give state feedback without using
    /// MessageBoxes, which steals the focus and thus influence the pop-up element)
    /// </summary>
    public string Status
    {
      get
      {
        return this.mStatus;
      }

      private set
      {
        if (this.mStatus != value)
        {
          this.mStatus = value;
          this.NotifyPropertyChanged(() => this.Status);
        }
      }
    }
    #endregion properties

    #region methods
    private void DropDownButtonClickCommand_Executed(ItemsItemViewModel p)
    {
      if (p == null)
        return;

      string source = "SplitItemsButton -> DropDownButtonClickCommand";

      this.Status = string.Format("Thanks for clicking: '{0} -> {1}'!", source, p.DisplayItemName);
    }

    private void ItemButtonClickCommand_Executed(ItemsItemViewModel p)
    {
      if (p == null)
        return;

      this.IsOpen = false;
      this.SelectedItem = p;

      string source = "SplitItemsButton -> ItemButtonClickCommand";

      this.Status += Environment.NewLine +
        string.Format("Thanks for clicking: '{0} -> {1}'!", source, p.DisplayItemName);
    }
    #endregion methods
  }
}
