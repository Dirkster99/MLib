namespace PDF_Binder.Converters
{
    using PDFBinderLib;
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// Converts a state into descriptive string that be used in a tool tip
    /// or other UI text item.
    /// </summary>
    [ValueConversion(typeof(PDFBinderLib.PDFTestResult), typeof(string))]
    public class StateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Binding.DoNothing;

            if ((value is PDFTestResult) == false)
                return Binding.DoNothing;

            var state = (PDFTestResult)value;

            switch (state)
            {
                case PDFTestResult.Unknown:
                    return "The state of the PDF file is unknown.";

                case PDFTestResult.OK:
                    return "The PDF file can be combined with other PDF files.";

                case PDFTestResult.Unreadable:
                    return "The PDF file is not readable (or may not exist).";

                case PDFTestResult.Protected:
                    return "The PDF file is protected (by a password or other means of security).";

                default:
                    throw new NotImplementedException(state.ToString());
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
