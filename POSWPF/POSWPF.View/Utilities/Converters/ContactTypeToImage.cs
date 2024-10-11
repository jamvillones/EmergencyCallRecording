using ECR.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using ECR.Domain.Models;
using System.Windows.Media.Imaging;

namespace ECR.WPF.Utilities.Converters {
    internal class ContactTypeToImage : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is ContactType type) {

                ImageSource image = null!;

                switch (type) {
                    case ContactType.Mobile:
                        image = new BitmapImage(new Uri("pack://application:,,,/Images/ContactTypes/phone_20px.png"));
                        break;
                    case ContactType.Telephone:
                        image = new BitmapImage(new Uri("pack://application:,,,/Images/ContactTypes/Rotary Dial Telephone_20px.png"));
                        break;
                    case ContactType.Email:
                        image = new BitmapImage(new Uri("pack://application:,,,/Images/ContactTypes/Email_20px.png"));
                        break;
                    case ContactType.Messenger:
                        image = new BitmapImage(new Uri("pack://application:,,,/Images/ContactTypes/Facebook Messenger_20px.png"));
                        break;

                    default:
                        break;
                }

                return image;
            }

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
