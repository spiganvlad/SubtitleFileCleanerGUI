using System;
using System.Windows.Data;
using System.Globalization;

namespace SubtitleFileCleanerGUI.Service
{
    public class ParameterConverter : IMultiValueConverter
    {
        // Allows to return more than one parameter from view
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Clone();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
