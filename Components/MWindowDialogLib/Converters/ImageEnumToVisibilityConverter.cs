namespace MWindowDialogLib.Converters
{
    using MWindowInterfacesLib.MsgBox.Enums;
    using System;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Markup;

    /// <summary>
    /// XAML mark up extension to convert an image enum value into a visibility value.
    /// This class is used to decide whether an image is shown or not.
    /// 
    /// The converter returns <seealso cref="System.Windows.Visibility.Visible"/>
    /// if the MsgBoxImage != MsgBoxImage.None, otherwise <seealso cref="System.Windows.Visibility.Collapsed"/>
    /// is returned.
    /// </summary>
    [MarkupExtensionReturnType(typeof(IValueConverter))]
  [ValueConversion(typeof(MsgBoxImage), typeof(Visibility))]
  public class ImageEnumToVisibilityConverter : MarkupExtension, IValueConverter
  {
    #region field
    private static ImageEnumToVisibilityConverter converter;
    #endregion field

    #region constructor
    /// <summary>
    /// Standard Constructor
    /// </summary>
    public ImageEnumToVisibilityConverter()
    {
    }
    #endregion constructor

    #region MarkupExtension
    /// <summary>
    /// When implemented in a derived class, returns an object that is provided
    /// as the value of the target property for this markup extension.
    /// 
    /// When a XAML processor processes a type node and member value that is a markup extension,
    /// it invokes the ProvideValue method of that markup extension and writes the result into the
    /// object graph or serialization stream. The XAML object writer passes service context to each
    /// such implementation through the serviceProvider parameter.
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
      if (converter == null)
      {
        converter = new ImageEnumToVisibilityConverter();
      }
  
      return converter;
    }
    #endregion MarkupExtension

    #region IValueConverter
    /// <summary>
    /// Null to visibility conversion method
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

      if ((value is MsgBoxImage) == false)
        return Binding.DoNothing;

      return SetImageSource((MsgBoxImage)value);
    }

    /// <summary>
    /// This function does the actual conversion from enum to <seealso cref="Visibility"/>.
    /// </summary>
    /// <param name="image"></param>
    /// <returns>Visibility.Collapsed for MsgBoxImage.None or Visibility.Visible for all others.</returns>
    public Visibility SetImageSource(MsgBoxImage image)
    {
      if (image == MsgBoxImage.None)
        return Visibility.Collapsed;
      
      return Visibility.Visible;
    }


    /// <summary>
    /// Visibility to Null conversion method (is not implemented)
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      return Binding.DoNothing;
    }
    #endregion IValueConverter
  }
}
