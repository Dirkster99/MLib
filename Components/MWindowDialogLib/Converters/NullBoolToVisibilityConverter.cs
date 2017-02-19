namespace MWindowDialogLib.Converters
{
    using System;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// XAML mark up extension to convert a null value into a visibility value.
    /// </summary>
    public class NullBoolToVisibilityConverter : IValueConverter
    {
        public NullBoolToVisibilityConverter()
        {
            True = Visibility.Visible;
            False = Visibility.Collapsed;
        }

        public Visibility True { get; set; }

        public Visibility False { get; set; }

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
                return this.False;

            if (value is bool)
            {
                if(((bool)value) == false)
                    return this.False;
            }

            return this.True;
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
