using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ECR.WPF.Utilities.Converters {
    internal class IsSameType : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            //get the value type
            //compare the type with the parameter

            Type valueType = value.GetType();
            Type paramaterType = parameter as Type;

            return valueType == paramaterType;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
