namespace DropDownButtonLib.ViewModels
{
  using System.Windows.Input;
  using DropDownButtonLib.Command;

  /// <summary>
  /// Implements a viewmodel that drives the
  /// the <seealso cref="DropDownButtonLib.Controls.DropDownButton"/> control.
  /// </summary>
  public class DropDownButtonViewModel : Base.BaseViewModel
  {
    #region fields
    private RelayCommand<object> mDropDownButtonClickCommand;
    private RelayCommand<object> mItemButtonClickCommand;
    private RelayCommand mItemCancelCommand;

    private bool mIsOpen;
    private bool mIsEnabled;
    private int mSliderValue;
    private string mStatus;
    private int mBackupSliderValue;
    #endregion fields

    #region constructor
    /// <summary>
    /// Class constructor
    /// </summary>
    public DropDownButtonViewModel()
    {
      this.mStatus = string.Empty;
      this.mBackupSliderValue = this.mSliderValue = 50;

      this.mIsEnabled = true;
      this.mIsOpen = false;
    }
    #endregion constructor

    #region properties
    #region commands
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
              if ((p is int) == false)
                return;

              var param = (int)p;

              this.DropDownButtonClickCommand_Executed(param);
            },
            (p) =>
            {
              return (this.IsEnabled == true);
            });

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
          this.mItemButtonClickCommand = new RelayCommand<object>(
                                 (p) =>
                                 {
                                   if ((p is int) == false)
                                     return;

                                   var param = (int)p;

                                    this.ItemButtonClickCommand_Executed(param);
                                 });

        return this.mItemButtonClickCommand;
      }
    }

    /// <summary>
    /// Gets a command that is executed to cancel a dropdown action.
    /// This command closes the drop down and rolls back edited values (if any).
    /// </summary>
    public ICommand ItemCancelCommand
    {
      get
      {
        if (this.mItemCancelCommand == null)
          this.mItemCancelCommand = new RelayCommand(
                                 () =>
                                 {
                                   this.ItemCancelCommand_Executed();
                                 });

        return this.mItemCancelCommand;
      }
    }
    #endregion commands

    /// <summary>
    /// Gets the selected item of the DropDown viewmodel.
    /// </summary>
    public string ButtonLabel
    {
      get
      {
        return "Select a Value";
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
    /// Gets/sets the current value displayed in the slider control.
    /// </summary>
    public int SliderValue
    {
      get
      {
        return this.mSliderValue;
      }

      set
      {
        if (this.mSliderValue != value)
        {
          this.mSliderValue = value;
          this.NotifyPropertyChanged(() => this.SliderValue);
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
    private void DropDownButtonClickCommand_Executed(int value)
    {
      string source = "SplitButton";

      this.Status = string.Format("Thanks for clicking me: '{0} -> {1}'!", source, value);
    }

    private void ItemButtonClickCommand_Executed(int value)
    {
      this.mBackupSliderValue = this.SliderValue;
      this.IsOpen = false;

      string source = "OK from SplitButtonItem";

      this.Status = string.Format("Thanks for clicking me: '{0} -> {1}'!", source, value);
    }

    private void ItemCancelCommand_Executed()
    {
      this.IsOpen = false;

      string source = "Cancel from SplitButtonItem";

      this.Status = string.Format("Cancel -> roll back from '{0} to {1} ({2})'!", this.SliderValue, this.mBackupSliderValue, source);

      this.SliderValue = this.mBackupSliderValue;
    }
    #endregion methods
  }
}
