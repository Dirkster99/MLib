namespace MLib.Converters
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media;

    /// <summary>
    /// Determines the Ideal Text Color Based on Specified Background Color
    /// http://www.codeproject.com/KB/GDI-plus/IdealTextColor.aspx
    /// </summary>
    public class BackgroundToForegroundConverter : IValueConverter, IMultiValueConverter
    {
        private static BackgroundToForegroundConverter _instance;

        /// <summary>
        /// Static class constructor
        /// Explicit static constructor to tell C# compiler
        /// not to mark type as beforefieldinit
        /// </summary>
        static BackgroundToForegroundConverter()
        {
        }

        /// <summary>
        /// Private class constructor
        /// </summary>
        private BackgroundToForegroundConverter()
        {
        }

        /// <summary>
        /// Gets another the static instance of this converter class.
        /// </summary>
        public static BackgroundToForegroundConverter Instance
        {
            get { return _instance ?? (_instance = new BackgroundToForegroundConverter()); }
        }

        /// <summary>
        /// Converts a Specified Background Color into an Ideal Text (Foreground) Color.
        /// http://www.codeproject.com/KB/GDI-plus/IdealTextColor.aspx
        /// </summary>
        /// <param name = "value"></param>
        /// <param name = "targetType"></param>
        /// <param name = "parameter"></param>
        /// <param name = "culture"></param>
        /// <returns>The converted object.</returns>
        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            if (value is SolidColorBrush)
            {
                var idealForegroundColor = this.IdealTextColor(((SolidColorBrush)value).Color);
                var foreGroundBrush = new SolidColorBrush(idealForegroundColor);
                foreGroundBrush.Freeze();
                return foreGroundBrush;
            }

            return Brushes.White;
        }

        /// <summary>
        /// Method is not implemented.
        /// </summary>
        /// <param name = "value"></param>
        /// <param name = "targetType"></param>
        /// <param name = "parameter"></param>
        /// <param name = "culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }

        /// <summary>
        /// Method expects an array of 2 objects:
        /// object[0] -> Background, object[1] -> Foreground
        ///
        /// and returns object[1] if it is not null
        /// or
        /// returns the ideal foreground color CONVERTED from object[0] value, otherwise.
        /// </summary>
        /// <param name = "values"></param>
        /// <param name = "targetType"></param>
        /// <param name = "parameter"></param>
        /// <param name = "culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var bgBrush = values.Length > 0 ? values[0] as Brush : null;
            var titleBrush = values.Length > 1 ? values[1] as Brush : null;

            if (titleBrush != null)
            {
                return titleBrush;
            }

            return Convert(bgBrush, targetType, parameter, culture);
        }

        /// <summary>
        /// Method is not implemented.
        /// </summary>
        /// <param name = "value"></param>
        /// <param name = "targetTypes"></param>
        /// <param name = "parameter"></param>
        /// <param name = "culture"></param>
        /// <returns></returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return targetTypes.Select(t => DependencyProperty.UnsetValue).ToArray();
        }
        

        /// <summary>
        /// Determining Ideal Text Color Based on Specified Background Color
        /// http://www.codeproject.com/KB/GDI-plus/IdealTextColor.aspx
        /// </summary>
        /// <param name = "bg">The bg.</param>
        /// <returns></returns>
        private Color IdealTextColor(Color bg)
        {
            const int nThreshold = 105;
            var bgDelta = System.Convert.ToInt32((bg.R * 0.299) + (bg.G * 0.587) + (bg.B * 0.114));
            var foreColor = (255 - bgDelta < nThreshold) ? Colors.Black : Colors.White;

            return foreColor;
        }
    }
}
