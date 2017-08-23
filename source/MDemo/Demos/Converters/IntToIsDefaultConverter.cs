namespace MDemo.Demos.Converters
{
    using System;
    using System.Windows.Data;

    /// <summary>
    /// XAML mark up extension to convert a null value into a visibility value.
    /// </summary>
    public class IntToIsDefaultConverter : IMultiValueConverter
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
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values == null)
                return false;

            try
            {
                if (values.Length != 2)
                    return false;

                int[] ivalues = new int[] { (int)values[0], (int)values[1] };

                if (ivalues != null)
                {
                    if (ivalues[0] == ivalues[1])
                        return true;
                }
            }
            catch
            {
            }

            return false;
        }

        /// <summary>
        /// Visibility to Null conversion method (is not implemented)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object[] ConvertBack(object values, Type[] targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
        #endregion IValueConverter
    }
}
