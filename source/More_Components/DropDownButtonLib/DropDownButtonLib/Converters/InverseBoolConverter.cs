/*************************************************************************************

   Extended WPF Toolkit

   Copyright (C) 2007-2013 Xceed Software Inc.

   This program is provided to you under the terms of the Microsoft Public
   License (Ms-PL) as published at http://wpftoolkit.codeplex.com/license 

   For more features, controls, and fast professional support,
   pick up the Plus Edition at http://xceed.com/wpf_toolkit

   Stay informed: follow @datagrid on Twitter or Like http://facebook.com/datagrids

  ***********************************************************************************/
namespace DropDownButtonLib.Converters
{
  using System;
  using System.Windows.Data;

  /// <summary>
  /// Implements a WPF <seealso cref="IValueConverter"/> that converts an input
  /// boolean value into an inverted boolean value (true to false, and vice versa).
  /// </summary>
  [ValueConversion(typeof(bool), typeof(bool))]
  public class InverseBoolConverter : IValueConverter
  {
    #region IValueConverter Members
    /// <summary>
    /// Standard Convert method of the <seealso cref="IValueConverter"/> interface.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      if (value == null)
        return Binding.DoNothing;

      if ((value is bool) == false)
        return Binding.DoNothing;

      return !(bool)value;
    }

    /// <summary>
    /// Standard ConvertBack method of the <seealso cref="IValueConverter"/> interface.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      if (value == null)
        return Binding.DoNothing;

      if ((value is bool) == false)
        return Binding.DoNothing;

      return !(bool)value;
    }
    #endregion
  }
}
