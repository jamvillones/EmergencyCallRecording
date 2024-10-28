using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ECR.WPF.Views.UserControls {
    /// <summary>
    /// Interaction logic for Pagination_View.xaml
    /// </summary>
    public partial class Pagination_View : UserControl {
        public Pagination_View() {
            InitializeComponent();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {

                var textbox = sender as TextBox;

                Keyboard.ClearFocus();

                if (int.TryParse(textbox!.Text, out int number) && number > 0) {
                    var binding = textbox!.GetBindingExpression(TextBox.TextProperty);
                    binding?.UpdateSource();
                }
                else {
                    textbox.Text = "100";
                    MessageBox.Show("Value must be a number and greater than 0!", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
