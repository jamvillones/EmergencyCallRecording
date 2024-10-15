using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ECR.WPF.Utilities.Converters {
    class GetFirstTwoInitialsFromNameConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is string name) {
                var spliced = name.Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(0).Take(2);

                var result = "";

                foreach (var s in spliced) {
                    result += s[0];
                }

                return result.ToUpper();
            }

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
