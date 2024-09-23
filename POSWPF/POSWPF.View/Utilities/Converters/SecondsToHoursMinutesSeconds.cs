using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ECR.WPF.Utilities.Converters {
    internal class SecondsToHoursMinutesSeconds : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            int secs = (int)value;

            TimeSpan t = TimeSpan.FromSeconds(secs);

            string answer = string.Format("{0:D2}h-{1:D2}m-{2:D2}s",
                            t.Hours,
                            t.Minutes,
                            t.Seconds);

            return answer;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
