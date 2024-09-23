using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ECR.View.Views {
    /// <summary>
    /// Interaction logic for BindablePasswordBox.xaml
    /// </summary>
    public partial class BindablePasswordBox : UserControl {

        public string Password {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PasswordProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register(
                "Password",
                typeof(string),
                typeof(BindablePasswordBox),
                new FrameworkPropertyMetadata(
                    string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    PasswordPropertyChanged,
                    null,
                    false,
                    UpdateSourceTrigger.PropertyChanged)
                );

        private static void PasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if (d is BindablePasswordBox pb) {
                pb.UpdatePassword();
            }
        }

        public bool IsPasswordHidden {
            get { return (bool)GetValue(IsPasswordHiddenProperty); }
            set { SetValue(IsPasswordHiddenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsPasswordHidden.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsPasswordHiddenProperty =
            DependencyProperty.Register("IsPasswordHidden", typeof(bool), typeof(BindablePasswordBox), new PropertyMetadata(false));


        public BindablePasswordBox() {
            InitializeComponent();
        }

        bool _isPasswordChanging = false;


        void UpdatePassword() {
            if (!_isPasswordChanging)
                passwordBox.Password = Password;
        }

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e) {
            _isPasswordChanging = true;
            Password = passwordBox.Password;
            _isPasswordChanging = false;
        }
    }
}
