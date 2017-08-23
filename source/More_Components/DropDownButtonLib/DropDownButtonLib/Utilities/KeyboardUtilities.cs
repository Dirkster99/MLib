/*************************************************************************************

   Extended WPF Toolkit

   Copyright (C) 2007-2013 Xceed Software Inc.

   This program is provided to you under the terms of the Microsoft Public
   License (Ms-PL) as published at http://wpftoolkit.codeplex.com/license 

   For more features, controls, and fast professional support,
   pick up the Plus Edition at http://xceed.com/wpf_toolkit

   Stay informed: follow @datagrid on Twitter or Like http://facebook.com/datagrids

  ***********************************************************************************/
namespace DropDownButtonLib.Utilities
{
  using System.Windows.Input;

  internal class KeyboardUtilities
  {
    /// <summary>
    /// Determine whether the <paramref name="e"/> parameter represents a keyboard short-cut
    /// that should modify the pop-up state of the drop-down button and return
    /// true if so, otherwise false.
    /// </summary>
    /// <param name="e"></param>
    /// <returns>true if keyboard short-cut should modify the pop-up state, otherwise false.</returns>
    internal static bool IsKeyModifyingPopupState(KeyEventArgs e)
    {
      return ((((Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt) &&
               ((e.SystemKey == Key.Down) || (e.SystemKey == Key.Up)))
               || (e.Key == Key.F4));
    }
  }
}
