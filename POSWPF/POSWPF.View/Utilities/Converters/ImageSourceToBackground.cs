using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace ECR.WPF.Utilities.Converters {
    internal class ImageSourceToBackground : IValueConverter {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture) {

            if (value is ImageSource image) {
                return new ImageBrush(image) { Stretch = Stretch.UniformToFill };
            }

            return default(ImageBrush);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}

