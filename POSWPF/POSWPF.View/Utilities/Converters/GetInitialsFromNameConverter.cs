using ECR.Domain.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ECR.WPF.Utilities.Converters {
    class GetInitialsFromNameConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is Name name) {

                List<char> intials = [name.First[0]];

                if (name.Middle is not null)
                    intials.Add(name.Middle[0]);

                if (name.Last is not null)
                    intials.Add(name.Last[0]);

                return string.Join("", intials);
            }

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
