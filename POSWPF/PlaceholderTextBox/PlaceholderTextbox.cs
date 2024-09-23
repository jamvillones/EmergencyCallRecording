using System.Windows;
using System.Windows.Controls;

namespace PlaceholderTextBox {

    public class PlaceholderTextbox : TextBox {
        static PlaceholderTextbox() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlaceholderTextbox), new FrameworkPropertyMetadata(typeof(PlaceholderTextbox)));
        }

        public string PlaceholderText {
            get { return (string)GetValue(PlaceholderTextProperty); }
            set { SetValue(PlaceholderTextProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.Register("PlaceholderText", typeof(string), typeof(PlaceholderTextbox), new PropertyMetadata(string.Empty));

        public bool IsEmpty {
            get { return (bool)GetValue(IsEmptyProperty); }
            private set { SetValue(IsEmptyPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey IsEmptyPropertyKey =
            DependencyProperty.RegisterReadOnly("IsEmpty", typeof(bool), typeof(PlaceholderTextbox), new PropertyMetadata(true));

        public static readonly DependencyProperty IsEmptyProperty = IsEmptyPropertyKey.DependencyProperty;

        protected override void OnTextChanged(TextChangedEventArgs e) {
            IsEmpty = string.IsNullOrEmpty(Text);
            base.OnTextChanged(e);
        }

        public CornerRadius CornerRadius {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(PlaceholderTextbox), new PropertyMetadata(new CornerRadius(0))
                );


    }
}
