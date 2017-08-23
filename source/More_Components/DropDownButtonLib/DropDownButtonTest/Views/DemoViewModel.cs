namespace DropDownButtonTest.Views
{
  using DropDownButtonLib.ViewModels;

  /// <summary>
  /// Implements an application viewmodel that can be used
  /// to drive the entire behaviour of the main application view.
  /// </summary>
  public class DemoViewModel : DropDownButtonLib.ViewModels.Base.BaseViewModel
  {
    #region fields
    private readonly SplitButtonViewModel mSplitButtonTest;
    private readonly DropDownItemsButtonViewModel mDropDownItemsButtonTest;
    private readonly DropDownButtonViewModel mDropDownButtonTest;
    private readonly SplitItemsButtonViewModel mSplitItemsButtonTest;
    #endregion fields

    #region constructor
    /// <summary>
    /// Class constructor
    /// </summary>
    public DemoViewModel()
    {
      this.mDropDownButtonTest = new DropDownButtonViewModel();
      this.mSplitButtonTest = new SplitButtonViewModel();
      this.mDropDownItemsButtonTest = new DropDownItemsButtonViewModel();
      this.mSplitItemsButtonTest = new SplitItemsButtonViewModel();
    }
    #endregion constructor

    #region properties
    /// <summary>
    /// Gets an instance of a <seealso cref="DropDownButtonViewModel"/>
    /// which can be bound to a <seealso cref="DropDownButtonLib.Controls.DropDownButton"/>
    /// control to test it.
    /// </summary>
    public DropDownButtonViewModel DropDownButtonTest
    {
      get
      {
        return this.mDropDownButtonTest;
      }
    }

    /// <summary>
    /// Gets an instance of a <seealso cref="SplitButtonViewModel"/>
    /// which can be bound to a <seealso cref="DropDownButtonLib.Controls.SplitButton"/>
    /// control to test it.
    /// </summary>
    public SplitButtonViewModel SplitButtonTest
    {
      get
      {
        return this.mSplitButtonTest;
      }
    }

    /// <summary>
    /// Gets an instance of a <seealso cref="SplitButtonViewModel"/>
    /// which can be bound to a <seealso cref="DropDownButtonLib.Controls.DropDownItemsButtonTest"/>
    /// control to test it.
    /// </summary>
    public DropDownItemsButtonViewModel DropDownItemsButtonTest
    {
      get
      {
        return this.mDropDownItemsButtonTest;
      }
    }

    public SplitItemsButtonViewModel SplitItemsButtonTest
    {
      get
      {
        return this.mSplitItemsButtonTest;
      }
    }    
    #endregion properties

    #region methods
    #endregion methods
  }
}
