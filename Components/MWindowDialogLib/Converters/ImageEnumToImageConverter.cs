namespace MWindowDialogLib.Converters
{
    using MWindowInterfacesLib.MsgBox.Enums;
    using System;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Markup;
    using System.Windows.Media.Imaging;
    using ViewModels;

    /// <summary>
    /// XAML mark up extension to convert a null value into a visibility value.
    /// This class is used to decide whether text document related controls,
    /// such as, highlighting patterns is shown or not.
    /// 
    /// The converter returns <seealso cref="System.Windows.Visibility.Visible"/>
    /// if the currently Active document is a text file that can be highlighted
    /// and <seealso cref="System.Windows.Visibility.Collapsed"/> otherwise.
    /// </summary>
    [MarkupExtensionReturnType(typeof(IValueConverter))]
  [ValueConversion(typeof(MsgBoxImage), typeof(string))]
  public class ImageEnumToImageConverter : MarkupExtension, IValueConverter
  {
    #region field
    private static ImageEnumToImageConverter converter;

    private static string[] msgBoxImageResourcesUris =
    {
       "48px-Emblem-important-yellow.svg.png",
       "48px-Help-browser.svg.png",
       "48px-Dialog-error-round.svg.png",
       "48px-Dialog-accept.svg.png",
       "48px-Software-update-urgent.svg.png",
       "48px-Dialog-information_on.svg.png",
       "48px-Emblem-notice.svg.png",

       // Advanced Icon Set
       "48px-Dialog-information.svg.png",
       "48px-Dialog-information_red.svg.png",
       "48px-Emblem-important.svg.png",
       "48px-Emblem-important-red.svg.png",
       "48px-Process-stop.svg.png"
    };
    #endregion field

    #region constructor
    /// <summary>
    /// Standard Constructor
    /// </summary>
    public ImageEnumToImageConverter()
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
        converter = new ImageEnumToImageConverter();
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
    /// This function does the actual conversion from enum to <seealso cref="BitmapImage"/>.
    /// </summary>
    /// <param name="image"></param>
    /// <returns></returns>
    public BitmapImage SetImageSource(MsgBoxImage image)
    {
      BitmapImage ret = null;

      switch (image)
      {
        case MsgBoxImage.Information:
          ret = this.GetApplicationResource("MsgBoxImage_Information");
          break;
        case MsgBoxImage.Question:
          ret = this.GetApplicationResource("MsgBoxImage_Question");
          break;
        case MsgBoxImage.Error:
          ret = this.GetApplicationResource("MsgBoxImage_Error");
          break;
        case MsgBoxImage.OK:
          ret = this.GetApplicationResource("MsgBoxImage_OK");
          break;
        case MsgBoxImage.Alert:
          ret = this.GetApplicationResource("MsgBoxImage_Alert");
          break;
        case MsgBoxImage.Default:
          ret = this.GetApplicationResource("MsgBoxImage_Default");
          break;
        case MsgBoxImage.Warning:
          ret = this.GetApplicationResource("MsgBoxImage_Warning");
          break;
        case MsgBoxImage.Default_OffLight:
          ret = this.GetApplicationResource("MsgBoxImage_Default_OffLight");
          break;
        case MsgBoxImage.Default_RedLight:
          ret = this.GetApplicationResource("MsgBoxImage_Default_RedLight");
          break;
        case MsgBoxImage.Information_Orange:
          ret = this.GetApplicationResource("MsgBoxImage_Information_Orange");
          break;
        case MsgBoxImage.Information_Red:
          ret = this.GetApplicationResource("MsgBoxImage_Information_Red");
          break;
        case MsgBoxImage.Process_Stop:
          ret = this.GetApplicationResource("MsgBoxImage_Process_Stop");
          break;
        case MsgBoxImage.None:
          return null;

        default:
          throw new NotImplementedException(image.ToString());
      }

      // just return dynamic resource if we found one
      // otherwise fall-through here and return back up image
      if (ret != null)
        return ret;

      string resourceAssembly = Assembly.GetAssembly(typeof(MsgBoxViewModel)).GetName().Name;

      string folder = "MsgBox/Images/MsgBoxImages/";

      // Tango Icon set: http://commons.wikimedia.org/wiki/Tango_icons
      // Default image displayed in message box
      string source = string.Format("pack://application:,,,/{0};component/{1}48px-Dialog-information_on.svg.png", resourceAssembly, folder);

      try
      {
        source = string.Format("pack://application:,,,/{0};component/{1}{2}",
                                resourceAssembly,
                                folder,
                                ImageEnumToImageConverter.msgBoxImageResourcesUris[(int)image]);
      }
      catch (Exception)
      {
      }

      Uri imageUri = new Uri(source, UriKind.RelativeOrAbsolute);
      
      return new BitmapImage(imageUri);
    }

    /// <summary>
    /// Attempt to locate a dynamic (<seealso cref="BitmapImage"/>) resource
    /// and return it or return null if the resource could not be located.
    /// </summary>
    /// <param name="resourceKey"></param>
    /// <returns></returns>
    private BitmapImage GetApplicationResource(string resourceKey)
    {
      try
      {
        if (Application.Current.Resources[resourceKey] != null)
        {
          if (Application.Current.Resources[resourceKey] is BitmapImage)
          {
            return Application.Current.Resources[resourceKey] as BitmapImage;
          }
        }
      }
      catch
      {
      }

      return null;
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
