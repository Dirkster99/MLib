namespace DropDownButtonLib.ViewModels.Items
{
  /// <summary>
  /// Implements a viewmodel that drives the
  /// the <seealso cref="DropDownButtonLib.Controls.DropDownButton"/> control.
  /// </summary>
  public class ItemsItemViewModel : DropDownButtonLib.ViewModels.Base.BaseViewModel
  {
    #region fields
    private string mDisplayItemName;
    #endregion fields

    #region constructor
    /// <summary>
    /// Parmeterized class constructor
    /// </summary>
    public ItemsItemViewModel(string displayName)
    : this()
    {
      this.mDisplayItemName = displayName;
    }

    /// <summary>
    /// Class constructor
    /// </summary>
    protected ItemsItemViewModel()
    {
      this.mDisplayItemName = null;
    }
    #endregion constructor

    #region properties
    /// <summary>
    /// Gets the string that represents a name for display purposes.
    /// </summary>
    public string DisplayItemName
    {
      get
      {
        return this.mDisplayItemName;
      }

      private set
      {
        if (this.mDisplayItemName != value)
        {
          this.mDisplayItemName = value;
          this.NotifyPropertyChanged(() => this.DisplayItemName);
        }
      }
    }
    #endregion properties
  }
}
