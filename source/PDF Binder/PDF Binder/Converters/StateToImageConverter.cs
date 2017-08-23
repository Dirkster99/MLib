namespace PDF_Binder.Converters
{
    using PDFBinderLib;
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    [ValueConversion(typeof(PDFBinderLib.PDFTestResult), typeof(System.Windows.Controls.Image))]
    public class StateToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Binding.DoNothing;

            if ((value is PDFTestResult) == false)
                return Binding.DoNothing;

            var state = (PDFTestResult)value;

            string typeURL = string.Empty;

            switch (state)
            {
                case PDFTestResult.Unknown:
                    typeURL = "fstate_unknown";
                    break;

                case PDFTestResult.OK:
                    typeURL = "fstate_OK";
                    break;

                case PDFTestResult.Unreadable:
                    typeURL = "fstate_unreadable";
                    break;

                case PDFTestResult.Protected:
                    typeURL = "fstate_locked";
                    break;

                default:
                    throw new NotImplementedException(state.ToString());
            }

            return Application.Current.FindResource(typeURL);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
