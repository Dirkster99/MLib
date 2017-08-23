/*************************************************************************************

   Extended WPF Toolkit

   Copyright (C) 2007-2013 Xceed Software Inc.

   This program is provided to you under the terms of the Microsoft Public
   License (Ms-PL) as published at http://wpftoolkit.codeplex.com/license 

   For more features, controls, and fast professional support,
   pick up the Plus Edition at http://xceed.com/wpf_toolkit

   Stay informed: follow @datagrid on Twitter or Like http://facebook.com/datagrids

  ***********************************************************************************/
namespace DropDownButtonLib.Controls
{
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Controls.Primitives;

  /// <summary>
  /// Implements a look-less WPF SplitButton control.
  /// </summary>
  [TemplatePart(Name = SplitButton.PART_ActionButton, Type = typeof(Button))]
  [TemplatePart(Name = SplitButton.PART_DropDownButton, Type = typeof(ToggleButton))]
  [TemplatePart(Name = SplitButton.PART_ContentPresenter, Type = typeof(ContentPresenter))]
  [TemplatePart(Name = SplitButton.PART_Popup, Type = typeof(Popup))]
  public class SplitButton : DropDownButton
  {
    #region fields
    /// <summary>
    /// Const string of the required additional button element in the <see cref="SplitButton"/> control.
    /// </summary>
    public const string PART_ActionButton = "PART_ActionButton";
    #endregion fields

    #region constructors
    /// <summary>
    /// Static class constructor
    /// </summary>
    static SplitButton()
    {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(SplitButton),
                                               new FrameworkPropertyMetadata(typeof(SplitButton)));
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
