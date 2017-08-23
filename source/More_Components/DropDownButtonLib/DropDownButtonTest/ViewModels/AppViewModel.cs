namespace DropDownButtonTest.ViewModels
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using DropDownButtonLib.ViewModels;
  using DropDownButtonTest.Views;

  /// <summary>
  /// Implements an application viewmodel that can be used
  /// to drive the entire behaviour of the main application view.
  /// </summary>
  public class AppViewModel : DropDownButtonLib.ViewModels.Base.BaseViewModel
  {
    #region fields
    private readonly DemoViewModel mDemo;
    #endregion fields

    #region constructor
    /// <summary>
    /// Class constructor
    /// </summary>
    public AppViewModel()
    {
      this.mDemo = new DemoViewModel();
    }
    #endregion constructor

    #region properties
    /// <summary>
    /// Gets an instance of a <seealso cref="DemoViewModel"/>
    /// which can be bound to the <seealso cref="DemoView"/>
    /// control to test functions implemented in the DropDownButtonLib.
    /// </summary>
    public DemoViewModel Demo
    {
      get
      {
        return this.mDemo;
      }
    }    
    #endregion properties

    #region methods
    #endregion methods
  }
}
