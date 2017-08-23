namespace PDF_Binder.Converters
{
	using System;
	using System.Windows.Data;

    /// <summary>
    /// XAML mark up extension to convert a null value into a visibility value.
    /// </summary>
    [ValueConversion(typeof(int), typeof(Boolean))]
    public class IntMinMaxToBooleanConverter : IValueConverter
    {
        /// <summary>
        /// Standard Constructor
        /// </summary>
        public IntMinMaxToBooleanConverter()
        {
            // By default 0 is mapped into false and
            // everything else > 0 is mapped to be true
            Min = Max = 0;
        }

        public int Min { get; set; }
        public int Max { get; set; }

        #region IValueConverter
        /// <summary>
        /// Zero to visibility conversion method
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return false;

            if (value is int)
            {
                if ((int)value >= Min && (int)value <= Max)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Visibility to Zero conversion method (is disabled and will throw an exception when invoked)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
        #endregion IValueConverter
    }
}
