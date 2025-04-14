using System.Globalization;
using System.Windows.Data;

namespace ECR.WPF.Utilities.Converters
{
    public class IsEmptyText : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
                return true;

            if (value is string s)            
                return string.IsNullOrEmpty(s);            

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
