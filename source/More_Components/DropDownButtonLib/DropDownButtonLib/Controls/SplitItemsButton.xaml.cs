namespace DropDownButtonLib.Controls
{
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Controls.Primitives;

  /// <summary>
  /// Implements a look-less WPF SplitItemsButton control.
  /// </summary>
  [TemplatePart(Name = PART_ActionButton, Type = typeof(Button))]
  [TemplatePart(Name = DropDownItemsButton.PART_DropDownButton, Type = typeof(ToggleButton))]
  [TemplatePart(Name = DropDownItemsButton.PART_ItemsControl, Type = typeof(ContentPresenter))]
  [TemplatePart(Name = DropDownItemsButton.PART_Popup, Type = typeof(Popup))]
  public class SplitItemsButton : DropDownItemsButton
  {
    #region fields
    /// <summary>
    /// Const string of the required additional button element in the <see cref="SplitItemsButton"/> control.
    /// </summary>
    public const string PART_ActionButton = "PART_ActionButton";
    #endregion fields

    #region constructors
    /// <summary>
    /// Static class constructor
    /// </summary>
    static SplitItemsButton()
    {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(SplitItemsButton),
                                               new FrameworkPropertyMetadata(typeof(SplitItemsButton)));
    }
    #endregion constructors

    #region methods
    /// <summary>
    /// <inheritedoc/>
    /// </summary>
    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      this.Button = GetTemplateChild(PART_ActionButton) as Button;
    }
    #endregion methods
  }
}
