namespace MWindowDialogLib.Converters
{
    using System;
    using System.Windows.Data;
    using System.Windows.Markup;

    /// <summary>
    /// XAML mark up extension to convert a null value into a visibility value.
    /// </summary>
    public class NullToVisibilityConverter : IValueConverter
    {
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
            {
                return System.Windows.Visibility.Collapsed;
            }

            if (value is string)
            {
                if((value as string).Length == 0)
                    return System.Windows.Visibility.Collapsed;
            }

            return System.Windows.Visibility.Visible;
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
