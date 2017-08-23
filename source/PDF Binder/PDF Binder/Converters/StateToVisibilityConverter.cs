namespace PDF_Binder.Converters
{
    using PDFBinderLib;
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// Converts a state <seealso cref="PDFBinderLib.PDFTestResult"/> enumeration into
    /// a visibility state such that unknown states are by default not shown.
    /// </summary>
    [ValueConversion(typeof(PDFBinderLib.PDFTestResult), typeof(Visibility))]
    public class StateToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Binding.DoNothing;

            if ((value is PDFTestResult) == false)
                return Binding.DoNothing;

            var state = (PDFTestResult)value;

            string typeURL = string.Empty;

            if (state == PDFTestResult.Unknown)
                    return Visibility.Hidden;

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
